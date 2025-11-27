namespace ManifestPatcher.Domain.Services
{
	public interface IManifestService
	{
		void UpdateFileHashesInsideManifest(string manifestPath, string newVersion);
	}
}
