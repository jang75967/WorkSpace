using System.Text.RegularExpressions;
using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.Manifest
{
	public class ClickOnceManifestService : IManifestService
	{
		private readonly IHashService _hashService;

		public ClickOnceManifestService(IHashService hashService)
		{
			_hashService = hashService;
		}

		public void UpdateFileHashesInsideManifest(string manifestPath, string newVersion)
		{
			string text = File.ReadAllText(manifestPath);
			string folder = Path.GetDirectoryName(manifestPath)!;

			bool updatedAssemblyIdentity = false;

			text = Regex.Replace(
				text,
				@"<(?<p>\w+:)?assemblyIdentity\b(?<attrs>[^>]*?)(?<end>\s*/?>)",
				m =>
				{
					if (updatedAssemblyIdentity)
						return m.Value;

					string attrs = m.Groups["attrs"].Value;

					var nameMatch = Regex.Match(
						attrs, @"\bname\s*=\s*""([^""]+)""", RegexOptions.IgnoreCase);
					if (!nameMatch.Success)
						return m.Value;

					string exeName = nameMatch.Groups[1].Value;
					string langValue =
						exeName.Contains("Configurator", StringComparison.OrdinalIgnoreCase) ||
						exeName.Contains("Admin", StringComparison.OrdinalIgnoreCase)
							? "en"
							: "en-US";

					if (Regex.IsMatch(attrs, @"\bversion\s*=\s*""[^""]+"""))
					{
						attrs = Regex.Replace(
							attrs,
							@"\bversion\s*=\s*""[^""]+""",
							$"version=\"{newVersion}\"",
							RegexOptions.IgnoreCase);
					}
					else
					{
						attrs += $" version=\"{newVersion}\"";
					}

					if (Regex.IsMatch(attrs, @"\blanguage\s*=\s*""[^""]+"""))
					{
						attrs = Regex.Replace(
							attrs,
							@"\blanguage\s*=\s*""[^""]+""",
							$"language=\"{langValue}\"",
							RegexOptions.IgnoreCase);
					}
					else
					{
						attrs += $" language=\"{langValue}\"";
					}

					Console.WriteLine($"assemblyIdentity 갱신: {exeName} → version={newVersion}, language={langValue}");

					updatedAssemblyIdentity = true;

					string prefix = m.Groups["p"].Success ? m.Groups["p"].Value : "";
					string end = m.Groups["end"].Value;
					return $"<{prefix}assemblyIdentity{attrs}{end}";
				},
				RegexOptions.IgnoreCase | RegexOptions.Singleline
			);

			text = Regex.Replace(
				text,
				@"<file[^>]*name=""([^""]+)""[^>]*>[\s\S]*?<dsig:DigestValue>([^<]+)</dsig:DigestValue>",
				m =>
				{
					string fileName = m.Groups[1].Value;
					string oldDigest = m.Groups[2].Value;
					string filePath = System.IO.Path.Combine(folder, fileName);

					if (!File.Exists(filePath))
					{
						Console.WriteLine($"누락 파일: {fileName}");
						return m.Value;
					}

					string newHash = _hashService.ComputeFileHashBase64(filePath);
					if (!string.Equals(newHash, oldDigest, StringComparison.Ordinal))
					{
						Console.WriteLine($"해시 갱신: {fileName}");
						return m.Value.Replace(oldDigest, newHash);
					}

					return m.Value;
				},
				RegexOptions.IgnoreCase | RegexOptions.Singleline
			);

			File.WriteAllText(manifestPath, text);
		}
	}
}
