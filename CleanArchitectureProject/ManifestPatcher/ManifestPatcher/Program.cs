using ManifestPatcher.Application;
using ManifestPatcher.Application.Pipeline;
using ManifestPatcher.Application.Steps;
using ManifestPatcher.Infrastructure.App;
using ManifestPatcher.Infrastructure.Hashing;
using ManifestPatcher.Infrastructure.Html;
using ManifestPatcher.Infrastructure.Manifest;
using ManifestPatcher.Infrastructure.Paths;
using ManifestPatcher.Infrastructure.Versioning;
using ManifestPatcher.Infrastructure.Zip;

class Program
{
	static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("사용법: ManifestPatcher.exe <배포 ZIP 경로>");
			return;
		}

		string zipPath = args[0];
		if (!File.Exists(zipPath))
		{
			Console.WriteLine($"ZIP 파일이 존재하지 않습니다: {zipPath}");
			return;
		}

		Console.Write("새버전 배포를 진행 하시겠습니까? (y/n)");
		string? confirmation = Console.ReadLine();
		if (!string.Equals(confirmation, "y", StringComparison.OrdinalIgnoreCase))
		{
			Console.WriteLine("취소되었습니다.");
			return;
		}

		var zipService = new ZipService();
		var appDetector = new AppDetector();
		var versioningService = new VersioningService();
		var hashService = new Sha256HashService();
		var manifestService = new ClickOnceManifestService(hashService);
		var pathProvider = new PathProvider();
		var indexHtmlService = new IndexHtmlService();

		var pipeline = new PipelineBuilder()
			.Use(new DetectTargetProgramStep(zipService, appDetector))
			.Use(new PreparePathsStep(pathProvider))
			.Use(new LoadAndBumpAppManifestStep(versioningService))
			.Use(new PrepareNewFolderAndExtractZipStep(zipService))
			.Use(new UpdateClientDllManifestStep(manifestService, pathProvider, hashService))
			.Use(new UpdateAppDependentAssemblyStep())
			.Use(new SaveManifestStep())
			.Use(new WriteIndexHtmlStep(indexHtmlService))
			.Build();

		var useCase = new RebuildManifestUseCase(pipeline);

		useCase.Execute(zipPath);
	}
}