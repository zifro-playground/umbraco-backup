using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Zifro.Models
{
    public class ChangePasswordViewModel
    {
	    [DisplayName("Nuvarande lösenord")]
	    [Required(ErrorMessage = "Lösenord saknas")]
	    public string CurrentPassword { get; set; }

		[DisplayName("Nytt lösenord")]
		[Required(ErrorMessage = "Lösenord saknas")]
		public string NewPassword { get; set; }

		[DisplayName("Välj ett lösenord.")]
		[Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
		public string ConfirmPassword { get; set; }
	}
}
