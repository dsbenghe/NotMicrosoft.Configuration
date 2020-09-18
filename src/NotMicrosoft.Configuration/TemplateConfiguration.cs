using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotMicrosoft.Configuration
{
    public class TemplateConfiguration
    {
        public static readonly TemplateConfiguration NopConfiguration = new TemplateConfiguration(new List<string>());
        private readonly string _environmentVariableName;

        private readonly List<string> _iniFilePaths;

        public TemplateConfiguration(
            IEnumerable<string> iniFilePaths,
            string environmentVariableName = "NOTMICROSOFT_CONFIG",
            char magicCharacter = '$')
        {
            if (iniFilePaths == null)
            {
                throw new ArgumentNullException(nameof(iniFilePaths));
            }

            if (string.IsNullOrWhiteSpace(environmentVariableName))
            {
                throw new ArgumentException("Expected NotNull and NotEmpty", nameof(environmentVariableName));
            }

            _iniFilePaths = new List<string>();
            _iniFilePaths.AddRange(iniFilePaths);
            _environmentVariableName = environmentVariableName;
            MagicCharacter = magicCharacter;
        }

        public char MagicCharacter { get; }

        public List<string> GetIniFilePaths()
        {
            var envVariableName = _environmentVariableName;
            var envIniFilePath = Environment.GetEnvironmentVariable(envVariableName);
            if (!string.IsNullOrWhiteSpace(envIniFilePath))
            {
                var iniFilePaths = envIniFilePath.Split(Path.PathSeparator).ToList();
                if (iniFilePaths.Any(x => !File.Exists(x))) throw new ArgumentException("Invalid IniFilePath.");
                return iniFilePaths;
            }

            if (_iniFilePaths.Any(x => !File.Exists(x))) throw new ArgumentException("Invalid IniFilePath.");

            return _iniFilePaths;
        }
    }
}