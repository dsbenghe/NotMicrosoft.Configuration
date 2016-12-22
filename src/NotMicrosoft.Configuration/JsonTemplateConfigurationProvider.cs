using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using Antlr4.StringTemplate;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateConfigurationProvider : JsonConfigurationProvider
    {
        private const int BufferSize = 1024;
        private readonly JsonTemplateConfigurationSource _jsonTemplateConfigurationSource;
        private JsonTemplateOptions _options;

        public JsonTemplateConfigurationProvider(JsonTemplateConfigurationSource jsonTemplateConfigurationSource)
            : base(jsonTemplateConfigurationSource)
        {
            _jsonTemplateConfigurationSource = jsonTemplateConfigurationSource;
        }

        public override void Load(Stream stream)
        {
            _options = new JsonTemplateOptions();
            _jsonTemplateConfigurationSource.Setup(_options);

            var configValues = GetConfigValues();
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
            var jsonSettings = ProcessJsonTemplate(GetTemplate(stream), configValues);
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

        private Template GetTemplate(Stream stream)
        {
            string templateString;
            using (var sr = new StreamReader(stream))
            {
                templateString = sr.ReadToEnd();
            }
            return new Template(templateString, _options.MagicCharacter,_options.MagicCharacter);
        }

        private string ProcessJsonTemplate(Template template, Dictionary<string, string> configValues)
        {
            foreach (var keyValuePair in configValues)
            {
                template.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return template.Render();
        }

        private Dictionary<string, string> GetConfigValues()
        {
            var iniFilePath = GetIniFilePath();
            if(iniFilePath != null)
                return IniParser.Parse(iniFilePath);
            return new Dictionary<string, string>();
        }

        private string GetIniFilePath()
        {
            var envVariableName = _options.EnvironmentVariableName;
            var envIniFilePath = Environment.GetEnvironmentVariable(envVariableName);
            if (!string.IsNullOrWhiteSpace(envIniFilePath) && File.Exists(envIniFilePath))
                return envIniFilePath;
            if (!string.IsNullOrWhiteSpace(_options.IniFilePath) && File.Exists(_options.IniFilePath))
                return _options.IniFilePath;
            return null;
        }
    }
}