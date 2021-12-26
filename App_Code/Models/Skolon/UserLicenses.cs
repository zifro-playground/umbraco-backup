using System;
using System.Collections.Generic;

namespace Zifro.Models.Skolon
{
	public class UserLicense
	{
		public string id { get; set; }
		public string appExtId { get; set; }
		public string target { get; set; }
		public string orderReference { get; set; }
		public string licenseCode { get; set; }
		public bool isDemo { get; set; }
		public DateTime expirationDate { get; set; }
	}

	public class UserLicenses
	{
		public List<UserLicense> licenses { get; set; }
	}
}