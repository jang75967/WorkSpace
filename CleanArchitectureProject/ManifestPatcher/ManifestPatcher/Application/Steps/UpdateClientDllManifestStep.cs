using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Steps
{
	public class UpdateClientDllManifestStep : IManifestRebuildStep
	{
		private readonly IManifestService _manifestService;
		private readonly IPathProvider _pathProvider;
		private readonly IHashService _hashService;

		public UpdateClientDllManifestStep(IManifestService manifestService, IPathProvider pathProvider, IHashService hashService)
		{
			_manifestService = manifestService;
			_pathProvider = pathProvider;
			_hashService = hashService;
		}

		public void Execute(ManifestRebuildContext context)
		{
			var clientManifestPath = Directory.GetFiles(context.NewFolderPath, $"Client.Apps.{context.TargetProgram}.dll.manifest", SearchOption.AllDirectories)
										.FirstOrDefault();
			if (clientManifestPath == null)
			{
				Console.WriteLine($"{context.TargetProgram} 매니페스트 파일을 찾지 못했습니다. (스킵)");
			}
			else
			{
				_manifestService.UpdateFileHashesInsideManifest(clientManifestPath, context.NewVersionString);
				Console.WriteLine($"{context.TargetProgram} 매니페스트 파일 해시 갱신 완료.");
			}

			// compute rel/abs
			string clientDllManifestRel = _pathProvider.GetClientDllManifestRelPath(context.NewFolderName, context.TargetProgram);
			string clientDllManifestAbs = _pathProvider.GetClientDllManifestAbsPath(context.BasePath, clientDllManifestRel);

			if (!File.Exists(clientDllManifestAbs))
			{
				clientDllManifestAbs = Directory.GetFiles(context.NewFolderPath, $"Client.Apps.{context.TargetProgram}.dll.manifest", SearchOption.AllDirectories)
											.FirstOrDefault() ?? "";
				if (!string.IsNullOrEmpty(clientDllManifestAbs))
				{
					var rel = Path.GetRelativePath(context.BasePath, clientDllManifestAbs);
					clientDllManifestRel = rel.Replace('/', '\\');
				}
			}

			if (!File.Exists(clientDllManifestAbs))
				throw new InvalidOperationException($"Client.Apps.{context.TargetProgram}.dll.manifest 파일을 찾을 수 없습니다.");

			long clientDllManifestSize = new FileInfo(clientDllManifestAbs).Length;
			string clientDllManifestHash = _hashService.ComputeFileHashBase64(clientDllManifestAbs);

			context.ClientDllManifestRel = clientDllManifestRel;
			context.ClientDllManifestAbs = clientDllManifestAbs;
			context.ClientDllManifestSize = clientDllManifestSize;
			context.ClientDllManifestHash = clientDllManifestHash;
		}
	}
}

