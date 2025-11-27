namespace ManifestPatcher.Domain.Services
{
	public interface IRebuildHandler
	{
		void Handle(ManifestRebuildContext context, Action next);
	}
}

