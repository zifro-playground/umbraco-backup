using System.Web.Mvc;
using Umbraco.Web.Mvc; 
 
namespace Zifro.Controllers
{
    public class ProfileController : SurfaceController
    {
        public ActionResult DeleteAccount()
        {
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
 
            if (userName == null)
            {
                ModelState.AddModelError("", "Kunde inte hitta någon inloggad användare. Kontakta support på help@zifro.se");
                return CurrentUmbracoPage();
            }
            //Get account information
            var memberService = Services.MemberService;
            var zifroAccount = memberService.GetByEmail(userName);
 
            //Log out first
            Members.Logout();
            //Delete account
            memberService.Delete(zifroAccount);
            return Redirect("/");
        }
    }
}