namespace WorkerService.Extensions
{
    public static class ConfigurationExtension
    {
        public static IConfigurationBuilder AddJsonFiles(this ConfigurationManager configurationManager)
        {
            return configurationManager
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("messagebussettings.json", optional: false, reloadOnChange: true);
        }
    }
}
