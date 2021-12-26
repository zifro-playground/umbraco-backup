using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.Models;
using Zifro.Code;

namespace Zifro.Controllers
{
	public class SkolonAuthPageController : RenderMvcController
	{
		public ActionResult Index(RenderModel model, string code)
		{
			var skolonUserCredentials = SkolonHelper.AuthenticateUser(code);

			//var userLicense = SkolonHelper.GetLicense(skolonUserCredentials);
			//TempData["data"] = userLicense;
			//return base.Index(model);

			var database = DatabaseContext.Database;
			var redirectPath = SkolonHelper.AuthorizeAndLoginUser(skolonUserCredentials, database);

			return Redirect(redirectPath);
		}
	}
}