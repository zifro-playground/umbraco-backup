using System.Net;
using System.Net.Mail;
using System.Web;

namespace Zifro.Code
{
	public class EmailHelper
	{
		private const string EmailFromAddress = "help@zifro.se";
		private const string EmailFromPassword = "GnUBhU7@W=-M";
		private const string Domain = "send.one.com";
		private const int Port = 587;

		public void SendResetPasswordEmail(string memberEmail, string resetGuid)
		{
			var baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
			var resetUrl = baseUrl + "/logga-in/återstaell-loesenord?resetGUID=" + resetGuid;

			var mail = new MailMessage();

			mail.From = new MailAddress(EmailFromAddress);
			mail.To.Add(memberEmail);
			mail.Subject = "Återställ lösenord";
			mail.IsBodyHtml = true;
			mail.Body = "<h3>Återställ lösenord</h3>" +
						"<p>Du har begärt att återställa ditt lösenord.<br/><br/>" +
			            "Om du inte har begärt att återställa lösenordet kan du ignorera detta mejl och radera det.</p>" +
			            "<p>Klicka här för att återställa ditt lösenord: " +
			            "<a href='" + resetUrl + "'>Återställ lösenord</a></p>" +
						"<p>Om du får problem kan du svara på detta mejl för att få hjälp!</p>";

			var client = new SmtpClient(Domain, Port);
			var netCred = new NetworkCredential(EmailFromAddress, EmailFromPassword);
			client.Credentials = netCred;
			client.EnableSsl = true;

			client.Send(mail);
		}

		public void SendResetPasswordEmailToNonUser(string memberEmail)
		{
			var baseUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
			var registerUrl = baseUrl + "/registrera";

			var mail = new MailMessage();

			mail.From = new MailAddress(EmailFromAddress);
			mail.To.Add(memberEmail);
			mail.Subject = "Återställ lösenord";
			mail.IsBodyHtml = true;
			mail.Body = "<h3>Återställ lösenord</h3>" +
						"<p>Du har begärt att återställa ditt lösenord men det finns inget konto registrerat med denna mejl-adress.<br/><br/>" +
			            "Om du inte har begärt att återställa lösenordet kan du ignorera detta mejl och radera det.</p>" +
			            "<p>Klicka här för att skapa ett nytt konto: " +
			            "<a href='" + registerUrl + "'>Skapa konto</a></p>";

			var client = new SmtpClient(Domain, Port);
			var netCred = new NetworkCredential(EmailFromAddress, EmailFromPassword);
			client.Credentials = netCred;
			client.EnableSsl = true;

			client.Send(mail);
		}
	}
}