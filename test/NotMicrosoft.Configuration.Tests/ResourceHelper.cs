using System.IO;
using System.Reflection;

namespace NotMicrosoft.Configuration.Tests
{
    public static class ResourceHelper
    {
        public static Stream GetResourceStream(string name)
        {
            var currentType = typeof(ResourceHelper).GetTypeInfo();
            var assembly = currentType.Assembly;
            var resourceStream = assembly.GetManifestResourceStream(currentType.Namespace + ".Resources." + name);
            return resourceStream;
        }
    }
}