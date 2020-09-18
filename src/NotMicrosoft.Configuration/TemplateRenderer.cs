using System;
using System.Collections.Generic;
using System.Globalization;
using Antlr4.StringTemplate;

namespace NotMicrosoft.Configuration
{
    public static class TemplateRenderer
    {
        public static string RenderTemplate(string template, TemplateConfiguration templateConfiguration,
            Dictionary<string, string> configValues)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("NotNull or NotEmpty expected", nameof(template));

            if (templateConfiguration == null) throw new ArgumentNullException(nameof(templateConfiguration));

            if (configValues == null) throw new ArgumentNullException(nameof(configValues));

            return ProcessJsonTemplate(GetTemplate(template, templateConfiguration), configValues);
        }

        private static Template GetTemplate(string template, TemplateConfiguration templateConfiguration)
        {
            return new Template(template, templateConfiguration.MagicCharacter, templateConfiguration.MagicCharacter);
        }

        private static string ProcessJsonTemplate(Template template, Dictionary<string, string> configValues)
        {
            foreach (var keyValuePair in configValues) template.Add(keyValuePair.Key, keyValuePair.Value);
            return template.Render(CultureInfo.InvariantCulture);
        }
    }
}