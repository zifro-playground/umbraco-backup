using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Zifro.Models
{
    public class ResetPasswordViewModel
    {
		[DisplayName("Epost")]
		[Required(ErrorMessage = "Skriv din epost-adress.")]
		[EmailAddress(ErrorMessage = "Ej giltig epost-adress")]
        public string Email { get; set; }

		[DisplayName("Lösenord")]
		[Required(ErrorMessage = "Lösenord saknas")]
		public string Password { get; set; }

		[DisplayName("Välj ett lösenord.")]
		[Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
		public string ConfirmPassword { get; set; }
	}
}
