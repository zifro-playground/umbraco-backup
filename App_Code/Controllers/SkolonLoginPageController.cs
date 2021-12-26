using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.Models;

namespace Zifro.Controllers
{
	public class SkolonLoginPageController : RenderMvcController
	{
		public ActionResult Index(RenderModel model)
		{
			var appBaseUrl = "https://www.zifro.se";
			if (Request.Url != null)
				appBaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority;

			var url = "https://idp.skolon.com/oauth/auth?" +
					  "client_id=s8NpPRy5lTAFv9q6gKNfTJD1NkHimffe&" +
					  "response_type=code&" +
					  "scope=authenticatedUser.identifier authenticatedUser.profile.read authenticatedUser.school.read authenticatedUser.groups.read authenticatedUser.licenses.read groups.members.read&" +
					  "redirect_uri=" + appBaseUrl + "/logga-in/skolon/auth";

			return Redirect(url);
		}
	}
}