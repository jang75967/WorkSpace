using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.Versioning
{
	public class VersioningService : IVersioningService
	{
		public Version IncrementRevision(Version version)
		{
			return new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
		}
	}
}
