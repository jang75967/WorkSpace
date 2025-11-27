namespace ManifestPatcher.Domain.Services
{
	public interface IManifestRebuildStep
	{
		void Execute(ManifestRebuildContext context);
	}
}
