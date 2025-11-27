namespace ManifestPatcher.Resources
{
	public static class IndexHtmlTemplate
	{
		public const string Template = @"<!doctype html>
<html lang=""ko""><head><meta charset=""utf-8"" /><title>{APP_TITLE}</title>
<style>body{font-family:Tahoma,Arial,sans-serif;margin:40px;color:#222}
.card{max-width:720px;border:1px solid #e5e7eb;border-radius:12px;box-shadow:0 4px 12px rgba(0,0,0,.06)}
.hd{background:#1c5280;color:#fff;padding:18px 22px;border-radius:12px 12px 0 0}.hd small{display:block;color:#cbd5e1}
.bd{padding:22px}.row{margin:8px 0}.label{display:inline-block;width:80px;color:#475569}
a.btn{display:inline-block;margin-top:12px;padding:10px 18px;background:#0078d7;color:#fff;text-decoration:none;border-radius:8px}
a.btn:hover{background:#005ea0}</style></head>
<body><div class=""card""><div class=""hd""><div style=""font-size:22px;font-weight:700"">{APP_TITLE}</div><small>jdg</small></div>
<div class=""bd""><div class=""row""><span class=""label"">Version</span> <span>{VERSION}</span></div>
<a class=""btn"" href=""setup.exe"">Install / Update</a></div></div></body></html>";
	}
}
