using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class WriteIndexHtmlStep : IManifestRebuildStep
	{
		private readonly IIndexHtmlService _indexHtmlService;

		public WriteIndexHtmlStep(IIndexHtmlService indexHtmlService)
		{
			_indexHtmlService = indexHtmlService;
		}

		public void Execute(ManifestRebuildContext context)
		{
			try
			{
				string indexHtml = _indexHtmlService.BuildIndexHtml(context.TargetProgram, context.NewVersionString);
				string? appManifestDir = Path.GetDirectoryName(context.AppManifestPath);
				if (!string.IsNullOrEmpty(appManifestDir))
				{
					string indexHtmlPath = Path.Combine(appManifestDir, "Index.html");
					File.WriteAllText(indexHtmlPath, indexHtml);
					Console.WriteLine($"Index.html 생성/갱신 완료: {indexHtmlPath}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Index.html 생성에 실패했습니다: {ex.Message}");
			}
		}
	}
}

