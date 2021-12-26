using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Security;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Zifro.Models.Playground.Api;
using Zifro.Models.Skolon;

namespace Zifro.Code
{
	public class SkolonHelper
	{
		public static HttpClient Client = new HttpClient();

		public static UserCredentials AuthenticateUser(string code)
		{
			var values = new Dictionary<string, string>
			{
				{ "code", code },
				{ "client_id", "s8NpPRy5lTAFv9q6gKNfTJD1NkHimffe" },
				{ "client_secret", "qRXjhmXyCv14QfnQTC3hzLnDtyGjzqp5BzpgDNwRB7FvCRtk" },
				{ "redirect_uri", "none" },
				{ "grant_type", "authorization_code" }
			};

			var content = new FormUrlEncodedContent(values);
			
			var response = Client.PostAsync("https://idp.skolon.com/oauth/access_token", content);
			var responseString = response.Result.Content.ReadAsStringAsync().Result;
			var userCredentials = JsonConvert.DeserializeObject<UserCredentials>(responseString);

			return userCredentials;
		}
		public static string AuthorizeAndLoginUser(UserCredentials credentials, Database database)
		{
			try
			{
				var license = ValidateLicense(credentials);
				if (license == null)
					return "/licens-saknas";

				var userIdentifier = GetUserIdentifier(credentials);
				var umbracoMember = MatchUserWithUmbracoMember(userIdentifier, credentials, database, license.isDemo);

				var redirectPath = LoginWithoutPassword(umbracoMember.Username);

				return redirectPath;
			}
			catch (Exception e)
			{
				LogHelper.Error(typeof(SkolonHelper), "Error logging in with skolon", e);
				return "/naagot-gick-fel";
			}
		}

		private static UserLicense ValidateLicense(UserCredentials credentials)
		{
			var currentAppLicense = GetLicense(credentials);

			if (currentAppLicense == null)
				return null;
			if (currentAppLicense.expirationDate.Date < DateTime.Today)
				return null;

			return currentAppLicense;
		}

		private static UserLicense GetLicense(UserCredentials credentials)
		{
			var urlLastPart = "licenses";
			var responseString = SendGetRequestToSkolonApi(urlLastPart, credentials);
			var userLicenses = JsonConvert.DeserializeObject<UserLicenses>(responseString);

			var currentAppLicense = userLicenses.licenses.Find(license => license.appExtId == "zifro-app");

			return currentAppLicense;
		}
		private static string GetUserIdentifier(UserCredentials credentials)
		{
			var urlLastPart = "identifier";
			var responseString = SendGetRequestToSkolonApi(urlLastPart, credentials);
			var userIdentifier = JsonConvert.DeserializeObject<UserIdentifier>(responseString);

			return userIdentifier.id;
		}
		private static UserProfile GetUserProfile(UserCredentials credentials)
		{
			var urlLastPart = "profile";
			var responseString = SendGetRequestToSkolonApi(urlLastPart, credentials);
			var userProfile = JsonConvert.DeserializeObject<UserProfile>(responseString);

			return userProfile;
		}


		private static IMember MatchUserWithUmbracoMember(string userIdentifier, UserCredentials credentials, Database database, bool isDemoLicense)
		{
			var memberService = ApplicationContext.Current.Services.MemberService;
			var userProfile = GetUserProfile(credentials);
			var member = memberService.GetByEmail(userProfile.email);

			var memberId = database.Fetch<int>("select UmbracoMemberId from UmbracoMemberToSkolonUser where SkolonUserId = '" + userIdentifier + "';");

			// If this is the first time the skolon user log in
			if (memberId == null || memberId.Count == 0)
			{
				// If the user does not already have an account on Zifro with the same email
				if (member == null)
				{
					member = CreateMember(userProfile, memberService, isDemoLicense);
				}

				var createSkolonUser = "insert into SkolonUser values('" + userIdentifier + "', '" + credentials.access_token + "', '" + credentials.refresh_token + "');";
				database.Execute(createSkolonUser);

				var mapMemberToSkolonUser = "insert into UmbracoMemberToSkolonUser values('" + member.Id + "', '" + userIdentifier + "');";
				database.Execute(mapMemberToSkolonUser);
			}
			else
			{
				var newSkolonUser = new SkolonUser();
				newSkolonUser.id = userIdentifier;
				newSkolonUser.accessToken = credentials.access_token;
				newSkolonUser.refreshToken = credentials.refresh_token;


				database.BeginTransaction(IsolationLevel.Serializable);

				int rowsEffected = database.Update("SkolonUser", "id", newSkolonUser, userIdentifier);
				if (rowsEffected == 0)
				{
					var insertNewUser = "insert into SkolonUser values('" + userIdentifier + "', '" + credentials.access_token +
					                    "', '" + credentials.refresh_token + "');";
					database.Execute(insertNewUser);
				}

				database.CompleteTransaction();
			}

			return member;
		}
		private static IMember CreateMember(UserProfile userProfile, IMemberService memberService, bool isDemoLicense)
		{
			var umbracoMemberRole = ConvertSkolonUserTypeToUmbracoMemberRole(userProfile.userType);

			var member = memberService.CreateMemberWithIdentity(userProfile.email, userProfile.email,
				userProfile.firstName + " " + userProfile.lastName, umbracoMemberRole);

			member.SetValue("acceptedUserAgreement", true);
			memberService.AssignRole(member.Id, umbracoMemberRole);
			memberService.AssignRole(member.Id, isDemoLicense ? "Trial" : "Premium");
			memberService.Save(member);

			return member;
		}
		private static string LoginWithoutPassword(string username)
		{
			FormsAuthentication.SetAuthCookie(username, true);

			var memberService = ApplicationContext.Current.Services.MemberService;
			var member = memberService.GetByEmail(username);
			var roles = memberService.GetAllRoles(member.Id).ToList();

			if (roles.Contains("Teacher"))
				return "/teacher";
			if (roles.Contains("Student"))
				return "/playground";

			return "/teacher";
		}

		private static string ConvertSkolonUserTypeToUmbracoMemberRole(string skolonUserType)
		{
			if (skolonUserType == "STUDENT")
				return "Student";
			if (skolonUserType == "TEACHER")
				return "Teacher";
			return "Other";
		}

		private static string SendGetRequestToSkolonApi(string urlLastPart, UserCredentials credentials)
		{
			var url = "https://api.skolon.com/v1/partner/authenticatedUser/" + urlLastPart;

			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Authorization", "Bearer " + credentials.access_token);
			
			var response = Client.SendAsync(request);
			var responseString = response.Result.Content.ReadAsStringAsync().Result;

			if (!response.Result.IsSuccessStatusCode)
				throw new Exception("Something went wrong when calling skolon API. " + responseString);

			return responseString;
		}
	}
}