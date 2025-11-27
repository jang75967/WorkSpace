using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.Html
{
	public class IndexHtmlService : IIndexHtmlService
	{
		public string BuildIndexHtml(string targetProgram, string versionString)
		{
			string appTitle = $"Client.Apps.{targetProgram}";
			string html = Resources.IndexHtmlTemplate.Template
				.Replace("{APP_TITLE}", appTitle)
				.Replace("{VERSION}", versionString);
			return html;
		}
	}
}