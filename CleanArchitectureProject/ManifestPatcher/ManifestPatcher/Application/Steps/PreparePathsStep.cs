using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class PreparePathsStep : IManifestRebuildStep
	{
		private readonly IPathProvider _pathProvider;

		public PreparePathsStep(IPathProvider pathProvider)
		{
			_pathProvider = pathProvider;
		}

		public void Execute(ManifestRebuildContext context)
		{
			if (string.IsNullOrWhiteSpace(context.TargetProgram))
				throw new InvalidOperationException("TargetProgram이 설정되지 않았습니다.");

			string basePath = _pathProvider.GetBasePath(context.TargetProgram);
			string appManifestPath = _pathProvider.GetAppManifestPath(basePath, context.TargetProgram);
			string appFilesDir = _pathProvider.GetAppFilesDir(basePath);

			if (!File.Exists(appManifestPath))
				throw new InvalidOperationException("application 파일을 찾을 수 없습니다.");

			context.BasePath = basePath;
			context.AppManifestPath = appManifestPath;
			context.AppFilesDir = appFilesDir;
		}
	}
}

