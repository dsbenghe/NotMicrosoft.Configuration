using System.IO;
using System.Linq;
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
            var templateString = new StreamReader(stream).ReadToEnd();
            var template = new Template(templateString);

            var iniValues = IniParser.Parse(_jsonTemplateConfigurationSource.IniFileName);
            foreach (var keyValuePair in iniValues)
            {
                template.Add(keyValuePair.Key, keyValuePair.Value);
            }
            var result = template.Render();

            using (var newStream = new MemoryStream())
            {
                using(var writer = new StreamWriter(newStream, Encoding.UTF8, BufferSize, true))
                {
                    writer.Write(result);
                }
                newStream.Seek(0, SeekOrigin.Begin);
                base.Load(newStream);
            }
        }
    }
}