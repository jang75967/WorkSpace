using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class DetectTargetProgramStep : IManifestRebuildStep
	{
		private readonly IZipService _zipService;
		private readonly IAppDetector _appDetector;

		public DetectTargetProgramStep(IZipService zipService, IAppDetector appDetector)
		{
			_zipService = zipService;
			_appDetector = appDetector;
		}

		public void Execute(ManifestRebuildContext context)
		{
			var manifestNames = _zipService.ListManifestEntryNames(context.ZipPath);
			string tartgetProgram = _appDetector.DetectTargetProgram(manifestNames);

			context.TargetProgram = tartgetProgram;
		}
	}
}

