namespace ManifestPatcher.Domain.Services
{
	public interface IHashService
	{
		string ComputeFileHashBase64(string filePath);
	}
}
