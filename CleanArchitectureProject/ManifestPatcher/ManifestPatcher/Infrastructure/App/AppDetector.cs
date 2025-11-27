using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.App
{
	public class AppDetector : IAppDetector
	{
		public string DetectTargetProgram(IEnumerable<string> manifestEntryNames)
		{
			var names = manifestEntryNames.ToList();
			if (names.Any(n => n.Contains("Configurator", StringComparison.OrdinalIgnoreCase)))
				return "Configurator";
			if (names.Any(n => n.Contains("Admin", StringComparison.OrdinalIgnoreCase)))
				return "AdminTool";
			return "Client";
		}
	}
}
