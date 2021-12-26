using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Zifro.Persistance.Migrations.Upgrades.TargetVersionOneZeroFour
{
	[Migration("1.0.4", 1, Code.Constants.Application.ApplicationName)]
	public class AddPlaygroundLevelProgressTable : MigrationBase
	{
		public AddPlaygroundLevelProgressTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{
		}
			
		public override void Up()
		{
			var tableName = "PlaygroundLevelProgress";
			var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
			if (tables.InvariantContains(tableName)) return;

			Create.Table(tableName)
				.WithColumn("UserId").AsInt32().NotNullable().ForeignKey("PK_PlaygroundLevelProgress_UserId", "cmsMember", "nodeId")
				.WithColumn("LevelId").AsString(50).ForeignKey("PK_PlaygroundLevelProgress_LevelId", "PlaygroundLevel", "LevelId")
				.WithColumn("IsComplete").AsBoolean().WithDefaultValue(false)
				.WithColumn("MainCode").AsCustom("nvarchar(MAX)").Nullable()
				.WithColumn("CodeLineCount").AsInt32().WithDefaultValue(0)
				.WithColumn("SecondsSpent").AsInt32().WithDefaultValue(0);

			Create.PrimaryKey("PK_PlaygroundLevelProgress").OnTable(tableName).Columns(new [] { "UserId", "LevelId" });
		}
			
		public override void Down()
		{
		}
	}
}