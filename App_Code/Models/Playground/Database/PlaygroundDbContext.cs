using System.Data.Entity;

namespace Zifro.Models.Playground.Database
{
	public partial class PlaygroundDbContext : DbContext
	{
		public PlaygroundDbContext()
			: base("name=umbracoDbDSN")
		{
		}

		public virtual DbSet<cmsMember> cmsMember { get; set; }
		public virtual DbSet<cmsPropertyData> cmsPropertyData { get; set; }
		public virtual DbSet<PlaygroundGame> PlaygroundGame { get; set; }
		public virtual DbSet<PlaygroundLevel> PlaygroundLevel { get; set; }
		public virtual DbSet<PlaygroundLevelProgress> PlaygroundLevelProgress { get; set; }
		public virtual DbSet<SkolonUser> SkolonUser { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<cmsMember>()
				.HasMany(e => e.PlaygroundLevelProgress)
				.WithRequired(e => e.cmsMember)
				.HasForeignKey(e => e.UserId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<cmsMember>()
				.HasMany(e => e.SkolonUser)
				.WithMany(e => e.cmsMember)
				.Map(m => m.ToTable("UmbracoMemberToSkolonUser").MapLeftKey("UmbracoMemberId").MapRightKey("SkolonUserId"));

			modelBuilder.Entity<cmsPropertyData>()
				.Property(e => e.dataDecimal)
				.HasPrecision(38, 6);

			modelBuilder.Entity<PlaygroundLevel>()
				.HasMany(e => e.PlaygroundLevelProgress)
				.WithRequired(e => e.PlaygroundLevel)
				.WillCascadeOnDelete(false);
		}
	}
}