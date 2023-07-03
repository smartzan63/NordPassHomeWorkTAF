using System.Reflection;

namespace NordPassHomeWorkTAF.Contexts
{
    public class JsonResourceContext
    {
        private readonly Assembly _assembly;

        public JsonResourceContext()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        public string GetJsonResource(string resourceName)
        {
            var resourceStream = _assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new ArgumentException($"Resource with name {resourceName} not found.");
            }

            using var reader = new StreamReader(resourceStream);
            return reader.ReadToEnd();
        }
    }

}
