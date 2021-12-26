using System;
using System.Linq;
using Semver;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Web;

namespace Zifro.Code
{
	internal class BootManager : ApplicationEventHandler
	{
		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			UpdateDatabase(Constants.Application.ApplicationName, "1.0.4");
			UpdateDatabase(Constants.Application.GameMigrationName, "1.0.0");
		}

		private void UpdateDatabase(string applicationName, string version)
		{
			var currentVersion = new SemVersion(0);

			// get all migrations already executed on DB
			var migrations =
				ApplicationContext.Current.Services.MigrationEntryService.GetAll(
					applicationName);

			// get the latest migration executed on DB
			var latestMigration = migrations.OrderByDescending(x => x.Version).FirstOrDefault();

			if (latestMigration != null)
			{
				currentVersion = latestMigration.Version;
			}

			var dbversion = new Version(version);

			var targetVersion = new SemVersion(
				dbversion.Major,
				dbversion.Minor,
				dbversion.Build,
				string.Empty,
				dbversion.Revision > 0 ? dbversion.Revision.ToInvariantString() : null);

			if (targetVersion == currentVersion)
			{
				// we are up to date
				return;
			}

			var migrationsRunner = new MigrationRunner(
				ApplicationContext.Current.Services.MigrationEntryService,
				ApplicationContext.Current.ProfilingLogger.Logger,
				currentVersion,
				targetVersion,
				applicationName);

			try
			{
				migrationsRunner.Execute(UmbracoContext.Current.Application.DatabaseContext.Database);
			}
			catch (Exception e)
			{
				// we catch all other errors
				LogHelper.Error(GetType(), "Error running migrations", e);
			}
		}
	}
}