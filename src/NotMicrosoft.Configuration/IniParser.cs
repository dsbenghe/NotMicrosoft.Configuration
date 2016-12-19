using System;
using System.Collections.Generic;
using System.IO;

namespace NotMicrosoft.Configuration
{
    public class IniParser
    {
        public static Dictionary<string, string> Parse(string iniFilePath)
        {
            using (var stream = new FileStream(iniFilePath, FileMode.Open, FileAccess.Read))
            {
                return Parse(stream);
            }
        }

        public static Dictionary<string, string> Parse(Stream stream)
        {
            var iniValues = new Dictionary<string, string>();
            using (var reader = new StreamReader(stream))
            {
                string readerLine;
                while ((readerLine = reader.ReadLine()) != null)
                {
                    var line = readerLine.Trim();

                    if (string.IsNullOrWhiteSpace(line) || line[0] == '#' /* comments */)
                    {
                        continue;
                    }

                    var separatorIndex = line.IndexOf('=');
                    if (separatorIndex < 0)
                    {
                        throw new ArgumentException("Invalid key value pair - missing =");
                    }

                    var key = line.Substring(0, separatorIndex).Trim();
                    var value = line.Substring(separatorIndex + 1).Trim();

                    iniValues[key] = value;
                }
            }
            return iniValues;
        }
    }
}
