using System;
using Microsoft.Extensions.Configuration;

namespace NotMicrosoft.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, Action<JsonTemplateOptions> setup = null)
        {
            return builder.AddJsonTemplateFile(path, false, setup);
        }

        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, bool optional, Action<JsonTemplateOptions> setup = null)
        {
            return builder.AddJsonTemplateFile(path, optional, false, setup);
        }

        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange, Action<JsonTemplateOptions> setup = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.", nameof(path));
            }

            var source = new JsonTemplateConfigurationSource(path, setup)
            {
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            return builder.Add(source);
        }
    }
}