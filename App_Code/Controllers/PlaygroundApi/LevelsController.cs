using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zifro.Models.Playground.Api;
using Zifro.Models.Playground.Database;
using Newtonsoft.Json;
using Umbraco.Web.WebApi;

namespace Zifro.Controllers
{
	public class LevelsController : UmbracoApiController
	{
		[Authorize]
		[HttpPost]
		public IHttpActionResult Save()
		{
			var postData = Request.Content.ReadAsStringAsync().Result;
			var jsonLevelProgress = JsonConvert.DeserializeObject<LevelProgress>(postData);

			using (var dbContext = new PlaygroundDbContext())
			{
				var level = dbContext.PlaygroundLevel.Find(jsonLevelProgress.levelId);
				if (level == null)
				{
					var message = string.Format("Could not find level with id = {0}", jsonLevelProgress.levelId);
					return BadRequest(message);
				}

				// use hard coded id like 1431 or 1449 while testing locally
				var currentMemberId = Members.GetCurrentMemberId();
				var currentMember = dbContext.cmsMember.Find(currentMemberId);

				if (currentMember == null)
					return Unauthorized();
				
				var levelProgress = dbContext.PlaygroundLevelProgress.Find(currentMember.nodeId, level.LevelId);

				if (levelProgress == null)
				{
					levelProgress = new PlaygroundLevelProgress
					{
						UserId = currentMember.nodeId,
						LevelId = jsonLevelProgress.levelId,
						IsComplete = jsonLevelProgress.isCompleted,
						MainCode = jsonLevelProgress.mainCode,
						CodeLineCount = jsonLevelProgress.codeLineCount,
						SecondsSpent = jsonLevelProgress.secondsSpent,
						cmsMember = currentMember,
						PlaygroundLevel = level
					};
					dbContext.PlaygroundLevelProgress.Add(levelProgress);
				}
				else
				{
					levelProgress.IsComplete = jsonLevelProgress.isCompleted;
					levelProgress.MainCode = jsonLevelProgress.mainCode;
					levelProgress.CodeLineCount = jsonLevelProgress.codeLineCount;
					levelProgress.SecondsSpent += jsonLevelProgress.secondsSpent;
				}

				dbContext.SaveChanges();
			}

			return Ok(string.Format("progress for level with id {0} have been saved", jsonLevelProgress.levelId));
		}

		[Authorize]
		[HttpGet]
		public HttpResponseMessage Load(string gameId)
		{
			using (var dbContext = new PlaygroundDbContext())
			{
				var currentMemberId = Members.GetCurrentMemberId();

				if (currentMemberId < 0)
				{
					var message = "Member with id = " + currentMemberId + " could not be found";
					return Request.CreateResponse(HttpStatusCode.Unauthorized, message);
				}

				var levels = dbContext.PlaygroundLevel.Where(x => x.GameId == gameId).ToList();

				if (!levels.Any())
				{
					var message = string.Format("Could not find progress for game with id = {0}", gameId);
					return Request.CreateResponse(HttpStatusCode.BadRequest, message);
				}

				var gameProgress = new GameProgress { levels = new List<LevelProgress>() };
				foreach (var level in levels)
				{
					var levelProgress = dbContext.PlaygroundLevelProgress.Find(currentMemberId, level.LevelId);

					if (levelProgress == null)
						continue;

					var levelProgressResponse = new LevelProgress()
					{
						levelId = levelProgress.LevelId,
						isCompleted = levelProgress.IsComplete,
						mainCode = levelProgress.MainCode,
						codeLineCount = levelProgress.CodeLineCount,
						secondsSpent = levelProgress.SecondsSpent
					};
					gameProgress.levels.Add(levelProgressResponse);
				}

				return Request.CreateResponse(HttpStatusCode.OK, gameProgress);
			}
		}
	}
}
