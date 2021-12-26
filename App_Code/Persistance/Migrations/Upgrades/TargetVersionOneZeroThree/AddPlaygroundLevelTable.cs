using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Zifro.Persistance.Migrations.Upgrades.TargetVersionOneZeroThree
{
	[Migration("1.0.3", 1, Code.Constants.Application.ApplicationName)]
	public class AddPlaygroundLevelTable : MigrationBase
	{
		public AddPlaygroundLevelTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{
		}

		public override void Up()
		{
			var tableName = "PlaygroundLevel";
			var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
			if (tables.InvariantContains(tableName)) return;

			Create.Table(tableName)
				.WithColumn("LevelId").AsString(50).PrimaryKey("PK_PlaygroundLevel_LevelId")
				.WithColumn("GameId").AsString(50).NotNullable()
				.ForeignKey("FK_PlaygroundLevel_PlaygroundGame", "PlaygroundGame", "GameId")
				.WithColumn("Precode").AsCustom("nvarchar(MAX)").Nullable();
		}

		public override void Down()
		{
		}
	}
}