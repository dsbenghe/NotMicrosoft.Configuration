using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotMicrosoft.Configuration.Parser
{
    public static class IniParser
    {
        public static Dictionary<string, string> Parse(List<string> iniFilePaths)
        {
            var iniStreams = iniFilePaths.Select(iniFile => new FileStream(iniFile, FileMode.Open, FileAccess.Read))
                .Cast<Stream>().ToList();
            var iniValues = Parse(iniStreams);
            foreach (var iniStream in iniStreams)
            {
                iniStream.Dispose();
            }
            return iniValues;
        }

        public static Dictionary<string, string> Parse(string iniFilePath)
        {
            using (var stream = new FileStream(iniFilePath, FileMode.Open, FileAccess.Read))
            {
                return Parse(stream);
            }
        }

        public static Dictionary<string, string> Parse(List<Stream> iniStreams)
        {
            return MergeDictionaries(iniStreams.Select(Parse));
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

        private static Dictionary<string, string> MergeDictionaries(
            IEnumerable<Dictionary<string, string>> dictionaries)
        {
            var finalDictionary = new Dictionary<string, string>();
            // SelectMany by definition preserves ordering
            dictionaries.SelectMany(x => x).ToList().ForEach(x => finalDictionary[x.Key] = x.Value);
            return finalDictionary;
        }
    }
}