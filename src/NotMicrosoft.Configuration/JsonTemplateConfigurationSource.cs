using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateConfigurationSource : JsonConfigurationSource
    {
        private readonly string _iniFileName;

        public JsonTemplateConfigurationSource(string path, string iniFileName)
        {
            Path = path;
            _iniFileName = iniFileName;
        }

        public string IniFileName => _iniFileName;

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new JsonTemplateConfigurationProvider(this);
        }
    }
}