using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zifro.Models
{
	public class ForgotPasswordModel
	{
		[DisplayName("Email")]
		[Required(ErrorMessage = "Skriv din mejl-adress")]
		[EmailAddress(ErrorMessage = "Ej giltig epost-adress")]
		public string Email { get; set; }
	}
}
