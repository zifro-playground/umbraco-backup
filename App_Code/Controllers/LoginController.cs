using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Zifro.Models;

namespace Zifro.Controllers
{
	public class LoginController : SurfaceController
	{
		public ActionResult Login(LoginModel model)
		{
			if (!ModelState.IsValid)
				return CurrentUmbracoPage();

			if (Members.Login(model.Email, model.Password))
			{
				var memberService = Services.MemberService;
				
				var member = memberService.GetByEmail(model.Email);
				var roles = memberService.GetAllRoles(member.Id);

				if (roles.Contains("Teacher"))
					return Redirect("/teacher");
				if (roles.Contains("Student"))
					return Redirect("/playground");
				else
					return Redirect("/teacher");
			}

			ModelState.AddModelError("", "Fel användarnamn eller lösenord.");

			return CurrentUmbracoPage();
		}

		public ActionResult Logout()
		{
			Members.Logout();
			return Redirect("/");
		}
	}
}