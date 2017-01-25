using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotMicrosoft.Configuration
{
    public class TemplateConfiguration
    {
        public static TemplateConfiguration NopConfiguration = new TemplateConfiguration(new List<string>());

        public TemplateConfiguration(IEnumerable<string> iniFilePaths, string environmentVariableName = "NOTMICROSOFT_CONFIG",
            char magicCharacter = '$')
        {
            IniFilePaths = new List<string>();
            IniFilePaths.AddRange(iniFilePaths);
            EnvironmentVariableName = environmentVariableName;
            MagicCharacter = magicCharacter;
        }

        public List<string> IniFilePaths { get; }
        public string EnvironmentVariableName { get; }
        public char MagicCharacter { get; }

        public List<string> GetIniFilePaths()
        {
            var envVariableName = EnvironmentVariableName;
            var envIniFilePath = Environment.GetEnvironmentVariable(envVariableName);
            if (!string.IsNullOrWhiteSpace(envIniFilePath))
            {
                var iniFilePaths = envIniFilePath.Split(Path.PathSeparator).ToList();
                if (iniFilePaths.Any(x => !File.Exists(x))) throw new ArgumentException("Invalid IniFilePath.");
                return iniFilePaths;
            }

            if (IniFilePaths.Any(x => !File.Exists(x))) throw new ArgumentException("Invalid IniFilePath.");

            return IniFilePaths;
        }
    }
}