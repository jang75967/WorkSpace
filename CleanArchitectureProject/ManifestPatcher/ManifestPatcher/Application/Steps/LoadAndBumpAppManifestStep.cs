using ManifestPatcher.Domain.Services;
using System.Xml.Linq;

namespace ManifestPatcher.Application.Steps
{
	public class LoadAndBumpAppManifestStep : IManifestRebuildStep
	{
		private readonly IVersioningService _versioningService;

		public LoadAndBumpAppManifestStep(IVersioningService versioningService)
		{
			_versioningService = versioningService;
		}

		public void Execute(ManifestRebuildContext context)
		{
			XNamespace asmV1 = "urn:schemas-microsoft-com:asm.v1";

			var appDoc = XDocument.Load(context.AppManifestPath);
			var appAsmIdentity = appDoc.Descendants(asmV1 + "assemblyIdentity").FirstOrDefault();
			if (appAsmIdentity == null)
				throw new InvalidOperationException("application 내 assemblyIdentity 태그를 찾을 수 없습니다.");

			var oldVersionStr = appAsmIdentity.Attribute("version")?.Value ?? "1.0.0.0";
			var oldVersion = Version.Parse(oldVersionStr);
			var newVersion = _versioningService.IncrementRevision(oldVersion);
			var newVersionStr = newVersion.ToString();

			appAsmIdentity.SetAttributeValue("version", newVersionStr);
			Console.WriteLine($"버전 갱신: {oldVersion} -> {newVersionStr}");

			context.AppDoc = appDoc;
			context.OldVersionString = oldVersionStr;
			context.NewVersionString = newVersionStr;
		}
	}
}

