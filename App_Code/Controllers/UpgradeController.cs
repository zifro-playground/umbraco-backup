using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Zifro.Models;

namespace Zifro.Controllers
{
	public class UpgradeController : SurfaceController
	{
		public ActionResult UpgradeToPremium(UpgradeViewModel model)
		{
			if (!ModelState.IsValid)
				return CurrentUmbracoPage();

			var memberService = Services.MemberService;
			var member = Members.GetCurrentMember();
			var roles = memberService.GetAllRoles(member.Id).ToList();

			if (roles.Contains("Premium"))
			{
				TempData["UpgradeSuccess"] = "Du är redan uppgraderad!";
				return CurrentUmbracoPage();
			}

			var validCodes = CurrentPage.GetPropertyValue<string[]>("codes");
			if (validCodes.Contains(model.Code, StringComparer.CurrentCultureIgnoreCase))
			{
				memberService.AssignRole(member.Id, "Premium");
				TempData["UpgradeSuccess"] = "Du har blivit uppgraderad!";
			}
			else
			{
				ModelState.AddModelError("", "Ogiltig kod.");
				return CurrentUmbracoPage();
			}

			if (roles.Contains("Trial"))
			{
				memberService.DissociateRole(member.Id, "Trial");
			}

			return CurrentUmbracoPage();
		}

		public ActionResult Logout()
		{
			Members.Logout();
			return Redirect("/");
		}
	}
}