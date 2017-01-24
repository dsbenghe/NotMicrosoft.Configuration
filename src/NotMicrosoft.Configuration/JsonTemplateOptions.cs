using System.Collections.Generic;

namespace NotMicrosoft.Configuration
{
    public class JsonTemplateOptions
    {
        public List<string> IniFilePaths { get; set; }
        public string EnvironmentVariableName { get; set; } = "NOTMICROSOFT_CONFIG";
        public char MagicCharacter { get; set; } = '$';
    }
}