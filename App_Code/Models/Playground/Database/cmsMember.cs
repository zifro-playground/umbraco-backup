using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Umbraco.Core.Models;


namespace Zifro.Models.Playground.Database
{
    [Table("cmsMember")]
    public partial class cmsMember
    {
        public cmsMember()
        {
            PlaygroundLevelProgress = new HashSet<PlaygroundLevelProgress>();
            SkolonUser = new HashSet<SkolonUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int nodeId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string LoginName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Password { get; set; }
		
        public virtual ICollection<PlaygroundLevelProgress> PlaygroundLevelProgress { get; set; }
		
        public virtual ICollection<SkolonUser> SkolonUser { get; set; }
    }
}
