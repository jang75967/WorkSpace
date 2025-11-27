using System.Security.Cryptography;
using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Infrastructure.Hashing
{
	public class Sha256HashService : IHashService
	{
		public string ComputeFileHashBase64(string filePath)
		{
			using var sha = SHA256.Create();
			using var stream = File.OpenRead(filePath);
			var hash = sha.ComputeHash(stream);
			return Convert.ToBase64String(hash);
		}
	}
}
