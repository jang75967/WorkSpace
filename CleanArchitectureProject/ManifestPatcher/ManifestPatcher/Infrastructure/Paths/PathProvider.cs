using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.Paths
{
	public class PathProvider : IPathProvider
	{
		public string GetBasePath(string targetProgram)
		{
#if PUBLISH_TEST
			return @$"D:\\PublishTest\\{targetProgram}";
#else
			return @$"D:\\Publish\\{targetProgram}";
#endif
		}

		public string GetAppManifestPath(string basePath, string targetProgram)
		{
			return Path.Combine(basePath, $"Client.Apps.{targetProgram}.application");
		}

		public string GetAppFilesDir(string basePath)
		{
			return Path.Combine(basePath, "Application Files");
		}

		public string GetNewFolderName(string targetProgram, string newVersion)
		{
			return $"Client.Apps.{targetProgram}_{newVersion.Replace('.', '_')}";
		}

		public string GetNewFolderPath(string appFilesDir, string newFolderName)
		{
			return Path.Combine(appFilesDir, newFolderName);
		}

		public string GetClientDllManifestRelPath(string newFolderName, string targetProgram)
		{
			return $@"Application Files\{newFolderName}\Client.Apps.{targetProgram}.dll.manifest";
		}

		public string GetClientDllManifestAbsPath(string basePath, string clientDllManifestRel)
		{
			return Path.Combine(basePath, clientDllManifestRel);
		}
	}
}
