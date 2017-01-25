﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration.Json;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateConfigurationProvider : JsonConfigurationProvider
    {
        private const int BufferSize = 1024;

        private JsonTemplateConfigurationSource ConfigurationSource => (JsonTemplateConfigurationSource)Source;

        public JsonTemplateConfigurationProvider(JsonTemplateConfigurationSource jsonTemplateConfigurationSource)
            : base(jsonTemplateConfigurationSource)
        {
        }

        public override void Load(Stream stream)
        {
            var configValues = ConfigurationSource.GetConfigValues();
            if (configValues.Count == 0)
            {
                base.Load(stream);
            }
            else
            {
                LoadViaTemplate(stream, configValues);
            }
        }

        private void LoadViaTemplate(Stream stream, Dictionary<string, string> configValues)
        {
            var jsonSettings = TemplateRenderer.RenderTemplate(GetTemplateString(stream), ConfigurationSource.TemplateConfiguration, configValues);
            using (var newStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(newStream, Encoding.UTF8, BufferSize, true))
                {
                    writer.Write(jsonSettings);
                }
                newStream.Seek(0, SeekOrigin.Begin);
                base.Load(newStream);
            }
        }

        private static string GetTemplateString(Stream stream)
        {
            string templateString;
            using (var sr = new StreamReader(stream))
            {
                templateString = sr.ReadToEnd();
            }
            return templateString;
        } 
    }
}