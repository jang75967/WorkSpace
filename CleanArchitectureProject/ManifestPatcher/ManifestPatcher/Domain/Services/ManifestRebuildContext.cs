using System.Xml.Linq;

namespace ManifestPatcher.Domain.Services
{
	public class ManifestRebuildContext
	{
		public string ZipPath { get; set; } = string.Empty;
		public string TargetProgram { get; set; } = string.Empty;
		public string BasePath { get; set; } = string.Empty;
		public string AppManifestPath { get; set; } = string.Empty;
		public string AppFilesDir { get; set; } = string.Empty;
		public XDocument? AppDoc { get; set; }
		public string NewVersionString { get; set; } = string.Empty;
		public string OldVersionString { get; set; } = string.Empty;
		public string NewFolderName { get; set; } = string.Empty;
		public string NewFolderPath { get; set; } = string.Empty;
		public bool ShouldReplaceIntegratedDbConfig { get; set; }
		public string? LocalIntegratedDbConfigPath { get; set; }
		public string ClientDllManifestRel { get; set; } = string.Empty;
		public string ClientDllManifestAbs { get; set; } = string.Empty;
		public long ClientDllManifestSize { get; set; }
		public string ClientDllManifestHash { get; set; } = string.Empty;
	}
}
