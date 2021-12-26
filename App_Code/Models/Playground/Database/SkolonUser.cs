using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Zifro.Models.Playground.Database
{
	[Table("SkolonUser")]
    public partial class SkolonUser
    {
        public SkolonUser()
        {
            cmsMember = new HashSet<cmsMember>();
        }

        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string AccessToken { get; set; }

        [Required]
        [StringLength(50)]
        public string RefreshToken { get; set; }
		
        public virtual ICollection<cmsMember> cmsMember { get; set; }
    }
}
