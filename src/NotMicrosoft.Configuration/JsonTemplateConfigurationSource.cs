using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateConfigurationSource : JsonConfigurationSource
    {
        private static readonly Action<JsonTemplateOptions> NopSetup = options => { };
        public JsonTemplateConfigurationSource()
        {
            Setup = NopSetup;
        }

        public JsonTemplateConfigurationSource(string path, Action<JsonTemplateOptions> setup) : this()
        {
            Setup = setup;
            Path = path;
        }

        public Action<JsonTemplateOptions> Setup { get; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new JsonTemplateConfigurationProvider(this);
        }
    }
}