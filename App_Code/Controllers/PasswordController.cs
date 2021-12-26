using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using Zifro.Models;
using Zifro.Code;

namespace Zifro.Controllers
{
	public class PasswordController : SurfaceController
	{
		public ActionResult HandleForgotPassword(ForgotPasswordModel model)
		{
			if (!ModelState.IsValid)
				return CurrentUmbracoPage();

			var memberService = Services.MemberService;
			var member = memberService.GetByEmail(model.Email);

			if (member != null)
			{
				DateTime expireTime = DateTime.Now.AddMinutes(30);
				string expireTimeFormated = expireTime.ToString("ddMMyyyyHHmmssFFFF");

				member.SetValue("resetGUID", expireTimeFormated);
				memberService.Save(member);

				var email = new EmailHelper();
				email.SendResetPasswordEmail(model.Email, expireTimeFormated);
			}
			else
			{
				var email = new EmailHelper();
				email.SendResetPasswordEmailToNonUser(model.Email);
			}

			TempData["EmailSendSuccess"] = "Ett mejl för att återställa lösenordet har skickats till " + model.Email;

			return CurrentUmbracoPage();
		}

		public ActionResult HandleResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return CurrentUmbracoPage();

			var memberService = Services.MemberService;
			var member = memberService.GetByEmail(model.Email);

			if (member == null)
			{
				ModelState.AddModelError("", "Återställning av lösenord har inte begärts för denna mejl-adress");
				return CurrentUmbracoPage();
			}

			var resetQuery = Request.QueryString["resetGUID"];

			if (string.IsNullOrEmpty(resetQuery))
			{
				ModelState.AddModelError("", "Återställning av lösenord har inte begärts för denna mejl-adress");
				return CurrentUmbracoPage();
			}

			var resetGUID = member.GetValue("resetGUID");
			if (resetGUID == null)
			{
				ModelState.AddModelError("", "Återställning av lösenord har inte begärts för denna mejl-adress");
				return CurrentUmbracoPage();
			}

			if (resetGUID.ToString() == resetQuery)
			{
				var expiryTime = DateTime.ParseExact(resetQuery, "ddMMyyyyHHmmssFFFF", null);
				var currentTime = DateTime.Now;

				if (currentTime < expiryTime)
				{
					memberService.SavePassword(member, model.Password);
					member.SetValue("resetGUID", string.Empty);
					memberService.Save(member);

					return Redirect("/logga-in");

				}
				else
				{
					ModelState.AddModelError("", "Länken till återställning har gått ut. Återställ lösenord igen.");
					return CurrentUmbracoPage();
				}
			}
			else
			{
				ModelState.AddModelError("", "Återställning av lösenord har inte begärts för denna mejl-adress");
				return CurrentUmbracoPage();
			}
		}

		public ActionResult HandleChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return CurrentUmbracoPage();

			var userName = System.Web.HttpContext.Current.User.Identity.Name;

			if (userName == null)
			{
				ModelState.AddModelError("", "Kunde inte hitta någon inloggad användare. Kontakta support på help@zifro.se");
				return CurrentUmbracoPage();
			}

			var memberService = Services.MemberService;
			var member = memberService.GetByEmail(userName);

			if (Membership.ValidateUser(member.Username, model.CurrentPassword))
			{
				memberService.SavePassword(member, model.NewPassword);
				memberService.Save(member);
				TempData["ChangePasswordSuccess"] = "Lösenordet har ändrats";
			}
			else
			{
				ModelState.AddModelError("", "Fel nuvarande lösenord");
			}
			TempData["ShowChangePwCard"] = true;
			return CurrentUmbracoPage();
		}
	}
}