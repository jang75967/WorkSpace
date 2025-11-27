using ManifestPatcher.Domain.Services;
using System.IO.Compression;

namespace ManifestPatcher.Infrastructure.Zip
{
	public class ZipService : IZipService
	{
		public List<string> ListManifestEntryNames(string zipPath)
		{
			using var archive = ZipFile.OpenRead(zipPath);
			return archive.Entries
				.Where(e => !string.IsNullOrEmpty(e.Name) && e.Name.EndsWith(".manifest", StringComparison.OrdinalIgnoreCase))
				.Select(e => e.Name)
				.ToList();
		}

		public bool Exists(string zipPath, string entryName)
		{
			using var archive = ZipFile.OpenRead(zipPath);
			return archive.Entries.Any(e =>
				!string.IsNullOrEmpty(e.Name) &&
				e.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
		}

		public void ExtractToDirectory(string zipPath, string destinationDirectory, bool ignoreCommonRoot = false)
		{
			if (!ignoreCommonRoot)
			{
				ZipFile.ExtractToDirectory(zipPath, destinationDirectory);
				return;
			}

			using var archive = ZipFile.OpenRead(zipPath);

			var fileEntries = archive.Entries
				.Where(e => !string.IsNullOrEmpty(e.Name))
				.ToList();

			if (fileEntries.Count == 0)
			{
				Directory.CreateDirectory(destinationDirectory);
				return;
			}

			var pathComponentsList = fileEntries
				.Select(e => e.FullName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
				.ToList();


			int minDirLen = pathComponentsList.Select(p => Math.Max(0, p.Length - 1)).DefaultIfEmpty(0).Min();
			int commonDirCount = 0;
			for (int i = 0; i < minDirLen; i++)
			{
				string candidate = pathComponentsList[0][i];
				bool allMatch = pathComponentsList.All(p => p[i].Equals(candidate, StringComparison.OrdinalIgnoreCase));
				if (!allMatch)
					break;
				commonDirCount++;
			}

			foreach (var entry in fileEntries)
			{
				var components = entry.FullName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				var relativeComponents = components.Skip(commonDirCount).ToArray();
				if (relativeComponents.Length == 0)
					continue;

				var relativePath = Path.Combine(relativeComponents);
				var destinationPath = Path.Combine(destinationDirectory, relativePath);
				var destinationDir = Path.GetDirectoryName(destinationPath);
				if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
					Directory.CreateDirectory(destinationDir);

				entry.ExtractToFile(destinationPath, overwrite: true);
			}
		}
	}
}
