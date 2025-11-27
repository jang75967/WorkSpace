namespace ManifestPatcher.Domain.Services
{
	public interface IPathProvider
	{
		string GetBasePath(string targetProgram);
		string GetAppManifestPath(string basePath, string targetProgram);
		string GetAppFilesDir(string basePath);
		string GetNewFolderName(string targetProgram, string newVersion);
		string GetNewFolderPath(string appFilesDir, string newFolderName);
		string GetClientDllManifestRelPath(string newFolderName, string targetProgram);
		string GetClientDllManifestAbsPath(string basePath, string clientDllManifestRel);
	}
}
