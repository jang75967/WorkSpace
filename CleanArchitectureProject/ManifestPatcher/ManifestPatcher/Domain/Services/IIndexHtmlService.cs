namespace ManifestPatcher.Domain.Services
{
	public interface IIndexHtmlService
	{
		string BuildIndexHtml(string targetProgram, string versionString);
	}
}