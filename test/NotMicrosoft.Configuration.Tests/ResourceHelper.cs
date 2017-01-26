using System;
using System.IO;
using System.Reflection;

namespace NotMicrosoft.Configuration.Tests
{
    public class ResourceHelper
    {
        public static Stream GetResurceStream(string name)
        {
            var currentType = typeof(ResourceHelper).GetTypeInfo();
            var assembly = currentType.Assembly;
            var resourceStream = assembly.GetManifestResourceStream(currentType.Namespace + ".Resources." + name);
            return resourceStream;
        }
    }
}
