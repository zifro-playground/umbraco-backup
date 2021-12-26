using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Zifro.Persistance.Migrations.Upgrades.TargetVersionOneZeroOne
{
	[Migration("1.0.1", 1, Code.Constants.Application.ApplicationName)]
	public class AddUmbracoMemberToSkolonUserTable : MigrationBase
	{
		public AddUmbracoMemberToSkolonUserTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{
		}
			
		public override void Up()
		{
			var tableName = "UmbracoMemberToSkolonUser";
			var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
			if (tables.InvariantContains(tableName)) return;

			Create.Table(tableName)
				.WithColumn("UmbracoMemberId").AsInt32().NotNullable().ForeignKey("FK_UmbracoMemberToSkolonUser_cmsMember", "cmsMember", "nodeId")
				.WithColumn("SkolonUserId").AsString(50).NotNullable().ForeignKey("FK_UmbracoMemberToSkolonUser_SkolonUser", "SkolonUser", "Id");

			Create.PrimaryKey("PK_UmbracoMemberToSkolonUser").OnTable(tableName).Columns(new[] { "UmbracoMemberId", "SkolonUserId" });
		}
			
		public override void Down()
		{
		}
	}
}