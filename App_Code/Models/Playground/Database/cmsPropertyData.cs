using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Zifro.Models.Playground.Database
{
	[Table("cmsPropertyData")]
    public partial class cmsPropertyData
    {
        public int id { get; set; }

        public int contentNodeId { get; set; }

        public Guid? versionId { get; set; }

        public int propertytypeid { get; set; }

        public int? dataInt { get; set; }

        public decimal? dataDecimal { get; set; }

        public DateTime? dataDate { get; set; }

        [StringLength(500)]
        public string dataNvarchar { get; set; }

        [Column(TypeName = "ntext")]
        public string dataNtext { get; set; }
    }
}
