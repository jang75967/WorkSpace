namespace ManifestPatcher.Domain.Services
{
	public interface IVersioningService
	{
		Version IncrementRevision(Version version);
	}
}
