using System.Collections.Generic;
using System.Linq;
using Zifro.Models.Playground.Database;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Zifro.Code;

namespace Zifro.Persistance.Migrations.GameUpgrades.TargetVersion_1_0_0
{
	[Migration("1.0.0", 1, Constants.Application.GameMigrationName)]
	public class UpdatePlaygroundGameData : MigrationBase
	{
		public UpdatePlaygroundGameData(ISqlSyntaxProvider sqlSyntax, ILogger logger)
			: base(sqlSyntax, logger)
		{}

		public override void Up()
		{
			using (var dbContext = new PlaygroundDbContext())
			{
				var game = dbContext.PlaygroundGame.Find("CARCONTROLLER");

				if (game == null)
					game = dbContext.PlaygroundGame.Add(new PlaygroundGame() {GameId = "CARCONTROLLER"});

				var levelsInDatabase = dbContext.PlaygroundLevel.Where(x => x.GameId == game.GameId);

				var levelsToUpdate = new List<string>
				{
					"CARCONTROLLER_HCNE1",
					"CARCONTROLLER_HCNE2",
					"CARCONTROLLER_HCNW1",
					"CARCONTROLLER_FORE1",
					"CARCONTROLLER_FORS1",
					"CARCONTROLLER_FORSE",
					"CARCONTROLLER_2FOR1",
					"CARCONTROLLER_FORD1",
					"CARCONTROLLER_FORD2",
					"CARCONTROLLER_FORDO",
					"CARCONTROLLER_FOR2D",
					"CARCONTROLLER_FOR2S",
				};

				var levelsPrecode = new Dictionary<string, string>()
				{
				};

				foreach (var levelToUpdate in levelsToUpdate)
				{
					var level = levelsInDatabase.FirstOrDefault(x => x.LevelId == levelToUpdate);
					var precode = levelsPrecode.ContainsKey(levelToUpdate) ? levelsPrecode[levelToUpdate] : null;

					if (level == null)
					{
						dbContext.PlaygroundLevel.Add(new PlaygroundLevel()
						{
							LevelId = levelToUpdate,
							GameId = game.GameId,
							Precode = precode,
							PlaygroundGame = game
						});
					}
					else
					{
						level.Precode = precode;
					}
				}

				dbContext.SaveChanges();
			}
		}

		public override void Down()
		{}
	}
}
