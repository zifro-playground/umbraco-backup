using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Zifro.Persistance.Migrations.Upgrades.TargetVersionOneZeroTwo
{
	[Migration("1.0.2", 1, Code.Constants.Application.ApplicationName)]
	public class AddPlaygroundGameTable : MigrationBase
	{
		public AddPlaygroundGameTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{
		}
			
		public override void Up()
		{
			var tableName = "PlaygroundGame";
			var tables = SqlSyntax.GetTablesInSchema(Context.Database).ToArray();
			if (tables.InvariantContains(tableName)) return;

			Create.Table(tableName)
				.WithColumn("GameId").AsString(50).PrimaryKey("PK_PlaygroundGame_GameId");
		}
			
		public override void Down()
		{
		}
	}
}