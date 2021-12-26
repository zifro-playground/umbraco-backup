using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zifro.Models
{
	public class LoginModel
	{
		[DisplayName("Email")]
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[DisplayName("Lösenord")]
		[Required]
		public string Password { get; set; }
	}
}
