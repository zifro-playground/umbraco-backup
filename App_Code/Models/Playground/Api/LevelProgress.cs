namespace Zifro.Models.Playground.Api
{
	public class LevelProgress
	{
		public string levelId { get; set; }
		public bool isCompleted { get; set; }
		public string mainCode { get; set; }
		public int codeLineCount { get; set; }
		public int secondsSpent { get; set; }
	}
}