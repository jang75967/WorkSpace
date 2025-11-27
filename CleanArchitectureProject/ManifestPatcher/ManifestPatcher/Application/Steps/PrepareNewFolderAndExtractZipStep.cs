using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class PrepareNewFolderAndExtractZipStep : IManifestRebuildStep
	{
		private readonly IZipService _zipService;
		private const string IntegratedDbConfigFileName = "IntegratedDBConfig.json";

		public PrepareNewFolderAndExtractZipStep(IZipService zipService)
		{
			_zipService = zipService;
		}

		public void Execute(ManifestRebuildContext context)
		{
			// compute new folder
			string sanitizedVersion = context.NewVersionString.Replace('.', '_');
			context.NewFolderName = $"Client.Apps.{context.TargetProgram}_{sanitizedVersion}";
			context.NewFolderPath = Path.Combine(context.AppFilesDir, context.NewFolderName);

			if (Directory.Exists(context.NewFolderPath))
				Directory.Delete(context.NewFolderPath, true);
			Directory.CreateDirectory(context.NewFolderPath);

			// config replace decision
			bool shouldReplaceIntegratedDbConfig = _zipService.Exists(context.ZipPath, IntegratedDbConfigFileName);
			string? localIntegratedDbConfigPath = null;

			if (shouldReplaceIntegratedDbConfig)
			{
				var exeDir = AppContext.BaseDirectory;
				localIntegratedDbConfigPath = Path.Combine(exeDir, IntegratedDbConfigFileName);
				if (!File.Exists(localIntegratedDbConfigPath))
					shouldReplaceIntegratedDbConfig = false;
			}

			context.ShouldReplaceIntegratedDbConfig = shouldReplaceIntegratedDbConfig;
			context.LocalIntegratedDbConfigPath = localIntegratedDbConfigPath;

			// extract
			_zipService.ExtractToDirectory(context.ZipPath, context.NewFolderPath, ignoreCommonRoot: true);
			Console.WriteLine($"압축 해제 완료: {context.NewFolderPath}");

			// replace config if needed
			if (context.ShouldReplaceIntegratedDbConfig && context.LocalIntegratedDbConfigPath != null)
			{
				var extractedIntegratedDbConfigPath = Directory
					.GetFiles(context.NewFolderPath, IntegratedDbConfigFileName, SearchOption.AllDirectories)
					.FirstOrDefault();

				if (extractedIntegratedDbConfigPath != null)
				{
					File.Copy(context.LocalIntegratedDbConfigPath, extractedIntegratedDbConfigPath, overwrite: true);
					Console.WriteLine("IntegratedDBConfig.json 파일을 실행 경로의 파일로 교체했습니다.");
				}
			}
		}
	}
}

