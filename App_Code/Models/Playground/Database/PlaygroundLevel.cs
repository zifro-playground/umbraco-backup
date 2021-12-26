using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zifro.Models.Playground.Database
{
	[Table("PlaygroundLevel")]
    public partial class PlaygroundLevel
    {
        public PlaygroundLevel()
        {
            PlaygroundLevelProgress = new HashSet<PlaygroundLevelProgress>();
        }

        [Key]
        [StringLength(50)]
        public string LevelId { get; set; }

		[Required]
		[StringLength(50)]
		public string GameId { get; set; }

		public string Precode { get; set; }

	    public virtual PlaygroundGame PlaygroundGame { get; set; }

		public virtual ICollection<PlaygroundLevelProgress> PlaygroundLevelProgress { get; set; }
    }
}
