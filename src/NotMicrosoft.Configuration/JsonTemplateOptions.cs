namespace NotMicrosoft.Configuration
{
    public class JsonTemplateOptions
    {
        public string IniFilePath { get; set; }
        public string EnvironmentVariableName { get; set; } = "NOTMICROSOFT_CONFIG";
    }
}