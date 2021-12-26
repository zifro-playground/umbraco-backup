using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zifro.Models
{
	public class UpgradeViewModel
	{
		[DisplayName("Kod")]
		[Required(ErrorMessage = "Skriv din kod för uppgradering.")]
		public string Code { get; set; }
	}
}
