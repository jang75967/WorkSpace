namespace ManifestPatcher.Domain.Services
{
	public interface IAppDetector
	{
		string DetectTargetProgram(IEnumerable<string> manifestEntryNames);
	}
}
