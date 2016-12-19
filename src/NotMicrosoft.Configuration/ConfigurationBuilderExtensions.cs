using Microsoft.Extensions.Configuration;

namespace NotMicrosoft.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, string iniFilePath)
        {
            var source = new JsonTemplateConfigurationSource(path, iniFilePath);
            return builder.Add(source);
        }
    }
}