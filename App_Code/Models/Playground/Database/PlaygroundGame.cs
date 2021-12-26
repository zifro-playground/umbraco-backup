using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zifro.Models.Playground.Database
{

	[Table("PlaygroundGame")]
    public partial class PlaygroundGame
    {
	    public PlaygroundGame()
	    {
		    PlaygroundLevel = new HashSet<PlaygroundLevel>();
	    }

		[Key]
        [StringLength(50)]
        public string GameId { get; set; }

	    public virtual ICollection<PlaygroundLevel> PlaygroundLevel { get; set; }
	}
}
