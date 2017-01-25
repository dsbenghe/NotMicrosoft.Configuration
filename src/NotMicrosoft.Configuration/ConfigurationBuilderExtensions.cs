using System;
using Microsoft.Extensions.Configuration;

namespace NotMicrosoft.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, TemplateConfiguration templateConfiguration = null)
        {
            return builder.AddJsonTemplateFile(path, false, templateConfiguration);
        }

        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, bool optional, TemplateConfiguration templateConfiguration = null)
        {
            return builder.AddJsonTemplateFile(path, optional, false, templateConfiguration);
        }

        public static IConfigurationBuilder AddJsonTemplateFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange, TemplateConfiguration templateConfiguration = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("File path must be a non-empty string.", nameof(path));
            }

            var source = new JsonTemplateConfigurationSource(path, templateConfiguration)
            {
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };

            return builder.Add(source);
        }
    }
}