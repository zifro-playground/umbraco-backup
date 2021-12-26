using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Zifro.Models
{
    public class RegisterViewModel
    {
		[DisplayName("Roll")]
		[Required(ErrorMessage = "Du måste välja någon roll.")]
	    public string MemberRole { get; set; }

		[DisplayName("Förnamn")]
        [Required(ErrorMessage = "Skriv ditt förnamn.")]
        public string FirstName { get; set; }

		[DisplayName("Efternamn")]
		[Required(ErrorMessage = "Skriv ditt efternamn.")]
		public string LastName { get; set; }

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

		[DisplayName("Användarvillkor")]
        [MustBeTrue(ErrorMessage = "Du måste acceptera användarvillkoren.")]
		public bool AcceptedUserAgreement { get; set; }

		public MemberRoleType MemberRoles { get; set; }
	}

	public enum MemberRoleType
	{
		Teacher,
		Student,
		Other
	}
}
