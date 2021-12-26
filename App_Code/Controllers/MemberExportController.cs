using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Zifro.Controllers
{
	public class MemberExportController : SurfaceController
	{
		public ActionResult ExportMembers(string memberType)
		{
            int pageSize = 20;
            int totalRecords;
            var members = ApplicationContext.Current.Services.MemberService.GetAll(0, pageSize, out totalRecords);

            var sb = new StringBuilder();
            sb.Append("sep=," + Environment.NewLine);
            sb.Append("Name,Email,School,Municipality,CreatedDate" + Environment.NewLine);
            
            int laps = (int)Math.Ceiling(totalRecords / (double)pageSize);
            for (int i = 0; i < laps; i++)
            {
                members = ApplicationContext.Current.Services.MemberService.GetAll(i, pageSize, out totalRecords);
                foreach (var member in members)
                {
                    var roles = Services.MemberService.GetAllRoles(member.Id);

                    if (roles.Contains(memberType))
                    {
                        var name = member.Name;
                        var email = member.Email;
                        var school = member.GetValue("schoolName");
                        var municipality = member.GetValue("municipality");
                        var createdDate = member.CreateDate.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");

                        sb.Append(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"" + Environment.NewLine,
                            name,
                            email,
                            school,
                            municipality,
                            createdDate
                        ));
                    }
                }
            }
            string csv = sb.ToString();

            return File(new UTF8Encoding().GetBytes(csv), "text/csv", "Registered" + memberType + "s.csv");
        }
	}
}