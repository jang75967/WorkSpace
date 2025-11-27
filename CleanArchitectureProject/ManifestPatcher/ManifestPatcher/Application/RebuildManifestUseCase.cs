using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application
{
	public class RebuildManifestUseCase
	{
		private readonly Action<ManifestRebuildContext> _pipeline;

		public RebuildManifestUseCase(Action<ManifestRebuildContext> pipeline)
		{
			_pipeline = pipeline;
		}

		public void Execute(string zipPath)
		{
			var context = new ManifestRebuildContext
			{
				ZipPath = zipPath
			};

			try
			{
				_pipeline(context);
				Console.WriteLine("모든 갱신 완료.");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
