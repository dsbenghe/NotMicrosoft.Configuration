using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateConfigurationSource : JsonConfigurationSource
    {
        public Action<JsonTemplateOptions> Setup { get; }

        public JsonTemplateConfigurationSource(string path, Action<JsonTemplateOptions> setup)
        {
            Setup = setup;
            Path = path;
        }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new JsonTemplateConfigurationProvider(this);
        }
    }
}