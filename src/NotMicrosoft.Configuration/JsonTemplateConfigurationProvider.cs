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

        public JsonTemplateConfigurationProvider(JsonTemplateConfigurationSource jsonTemplateConfigurationSource)
            : base(jsonTemplateConfigurationSource)
        {
            _jsonTemplateConfigurationSource = jsonTemplateConfigurationSource;
        }

        public override void Load(Stream stream)
        {
            var jsonSettings = ProcessJsonTemplate(GetTemplate(stream));

            using (var newStream = new MemoryStream())
            {
                using(var writer = new StreamWriter(newStream, Encoding.UTF8, BufferSize, true))
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
            return new Template(templateString);
        }

        private string ProcessJsonTemplate(Template template)
        {
            var iniValues = GetConfigValues();
            foreach (var keyValuePair in iniValues)
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
            var options = new JsonTemplateOptions();
            _jsonTemplateConfigurationSource.Setup(options);
            var envVariableName = options.EnvironmentVariableName;
            var envIniFilePath = Environment.GetEnvironmentVariable(envVariableName);
            if (!string.IsNullOrWhiteSpace(envIniFilePath) && File.Exists(envIniFilePath))
                return envIniFilePath;
            if (!string.IsNullOrWhiteSpace(options.IniFilePath) && File.Exists(options.IniFilePath))
                return options.IniFilePath;
            return null;
        }
    }
}