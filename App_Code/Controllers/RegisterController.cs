using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Zifro.Models;

namespace Zifro.Controllers
{
    public class RegisterController : SurfaceController
    {
	    public ActionResult Register(RegisterViewModel model)
	    {
		    if (!ModelState.IsValid)
			    return CurrentUmbracoPage();

            var memberService = Services.MemberService;

		    if (memberService.GetByEmail(model.Email) != null)
			    ModelState.AddModelError("", "Det finns redan ett konto med den epost-adressen.");

		    if (!ModelState.IsValid)
			    return CurrentUmbracoPage();

			var member = memberService.CreateMemberWithIdentity(model.Email, model.Email,
			    model.FirstName + " " + model.LastName, model.MemberRole);

            member.SetValue("acceptedUserAgreement", true);
		    memberService.SavePassword(member, model.Password);
		    memberService.AssignRole(member.Id, "Trial");
		    memberService.AssignRole(member.Id, model.MemberRole);
		    memberService.Save(member);

		    Members.Login(model.Email, model.Password);

			if (model.MemberRole == "Teacher" || model.MemberRole == "Other")
				return Redirect("/teacher");
			else if (model.MemberRole == "Student")
				return Redirect("/playground");
			else
				return Redirect("/");
		}
	}
}