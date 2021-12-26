using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zifro.Models.Playground.Database
{
	[Table("PlaygroundLevelProgress")]
    public partial class PlaygroundLevelProgress
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string LevelId { get; set; }

        public bool IsComplete { get; set; }

        public string MainCode { get; set; }

        public int CodeLineCount { get; set; }

		public int SecondsSpent { get; set; }

		public virtual cmsMember cmsMember { get; set; }

        public virtual PlaygroundLevel PlaygroundLevel { get; set; }
	}
}
