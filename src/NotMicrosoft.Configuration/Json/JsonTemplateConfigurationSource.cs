using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using NotMicrosoft.Configuration.Parser;

namespace NotMicrosoft.Configuration.Json
{
    public class JsonTemplateConfigurationSource : JsonConfigurationSource
    {
        public JsonTemplateConfigurationSource()
        {
            TemplateConfiguration = TemplateConfiguration.NopConfiguration;
        }

        public JsonTemplateConfigurationSource(string path, TemplateConfiguration templateConfiguration) : this()
        {
            TemplateConfiguration = templateConfiguration;
            Path = path;
        }

        public TemplateConfiguration TemplateConfiguration { get; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new JsonTemplateConfigurationProvider(this);
        }

        public virtual Dictionary<string, string> GetConfigValues()
        {
            return IniParser.Parse(TemplateConfiguration.GetIniFilePaths());
        }
    }
}