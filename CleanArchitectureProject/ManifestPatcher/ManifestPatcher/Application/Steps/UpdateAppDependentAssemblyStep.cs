using ManifestPatcher.Domain.Services;
using System.Xml.Linq;

namespace ManifestPatcher.Application.Steps
{
	public class UpdateAppDependentAssemblyStep : IManifestRebuildStep
	{
		public void Execute(ManifestRebuildContext context)
		{
			if (context.AppDoc == null)
				throw new InvalidOperationException("AppDoc가 로드되지 않았습니다.");

			XNamespace asmV1 = "urn:schemas-microsoft-com:asm.v1";
			XNamespace asmV2 = "urn:schemas-microsoft-com:asm.v2";
			XNamespace dsig = "http://www.w3.org/2000/09/xmldsig#";

			var depAssembly = context.AppDoc
				.Descendants(asmV2 + "dependentAssembly")
				.FirstOrDefault(x =>
				{
					var ai = x.Element(asmV2 + "assemblyIdentity");
					return ai != null && (string?)ai.Attribute("name") == $"Client.Apps.{context.TargetProgram}.exe";
				});

			if (depAssembly == null)
				throw new InvalidOperationException(".application 내 asmv2:dependentAssembly (Client.exe) 를 찾지 못했습니다.");

			depAssembly.SetAttributeValue("codebase", context.ClientDllManifestRel);
			depAssembly.SetAttributeValue("size", context.ClientDllManifestSize.ToString());

			var depAsmIdentity = depAssembly.Element(asmV2 + "assemblyIdentity");
			if (depAsmIdentity != null)
				depAsmIdentity.SetAttributeValue("version", context.NewVersionString);

			var digestValueNode = depAssembly
				.Elements(asmV2 + "hash").Descendants(dsig + "DigestValue")
				.FirstOrDefault();

			if (digestValueNode == null)
			{
				digestValueNode = depAssembly
					.Elements(asmV1 + "hash").Descendants(dsig + "DigestValue")
					.FirstOrDefault();
			}
			if (digestValueNode == null)
			{
				var hashElem = depAssembly.Element(asmV2 + "hash") ?? new XElement(asmV2 + "hash");
				if (hashElem.Parent == null) depAssembly.Add(hashElem);

				var transforms = new XElement(dsig + "Transforms",
								new XElement(dsig + "Transform",
									new XAttribute("Algorithm", "urn:schemas-microsoft-com:HashTransforms.Identity")));
				var method = new XElement(dsig + "DigestMethod",
							new XAttribute("Algorithm", "http://www.w3.org/2000/09/xmldsig#sha256"));
				var digestVal = new XElement(dsig + "DigestValue", context.ClientDllManifestHash);

				hashElem.RemoveNodes();
				hashElem.Add(transforms);
				hashElem.Add(method);
				hashElem.Add(digestVal);
			}
			else
			{
				digestValueNode.Value = context.ClientDllManifestHash;
			}

			Console.WriteLine("application 내 dependentAssembly(codebase/size/version/DigestValue) 갱신 완료.");
		}
	}
}

