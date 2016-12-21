using System;
using Microsoft.Extensions.Configuration;

namespace NotMicrosoft.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, Action<JsonTemplateOptions> setup)
        {
            var source = new JsonTemplateConfigurationSource(path, setup);
            return builder.Add(source);
        }
    }
}