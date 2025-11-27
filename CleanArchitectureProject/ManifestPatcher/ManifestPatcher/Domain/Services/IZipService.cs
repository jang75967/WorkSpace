namespace ManifestPatcher.Domain.Services
{
	public interface IZipService
	{
		List<string> ListManifestEntryNames(string zipPath);
		bool Exists(string zipPath, string entryName);
		void ExtractToDirectory(string zipPath, string destinationDirectory, bool ignoreCommonRoot);
	}
}
