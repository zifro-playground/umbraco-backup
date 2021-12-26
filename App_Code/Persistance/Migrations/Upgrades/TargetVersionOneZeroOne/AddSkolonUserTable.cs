using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Zifro.Persistance.Migrations.Upgrades.TargetVersionOneZeroOne
{
	[Migration("1.0.1", 1, Code.Constants.Application.ApplicationName)]
	public class AddSkolonUserTable : MigrationBase
	{
		public AddSkolonUserTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{
		}
			
		public override void Up()
		{
			var tableName = "SkolonUser";
			var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
			if (tables.InvariantContains(tableName)) return;

			Create.Table(tableName)
				.WithColumn("Id").AsString(50).PrimaryKey("PK_SkolonUser_Id")
				.WithColumn("AccessToken").AsString(50).NotNullable()
				.WithColumn("RefreshToken").AsString(50).NotNullable();
		}
			
		public override void Down()
		{
		}
	}
}