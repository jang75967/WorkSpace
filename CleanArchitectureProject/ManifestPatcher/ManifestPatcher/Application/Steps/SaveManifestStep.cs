using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class SaveManifestStep : IManifestRebuildStep
	{
		public void Execute(ManifestRebuildContext context)
		{
			if (context.AppDoc == null)
				throw new InvalidOperationException("AppDoc가 로드되지 않았습니다.");

			context.AppDoc.Save(context.AppManifestPath);
			Console.WriteLine("application 매니페스트 저장 완료.");
		}
	}
}

