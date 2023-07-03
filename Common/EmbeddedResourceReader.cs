using System.Reflection;
namespace NordPassHomeWorkTAF.Common
{
    public static class EmbeddedResourceReader
    {
        public static string ReadAsString(string resourceName, Assembly assembly)
        {
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assembly.FullName}'.");
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static Stream ReadAsStream(string resourceName, Assembly assembly)
        {
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new ArgumentException($"Resource '{resourceName}' not found in assembly '{assembly.FullName}'.");
            }

            return stream;
        }
    }
}
