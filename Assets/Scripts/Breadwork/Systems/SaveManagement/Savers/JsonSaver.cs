using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scripts.Core;
using Unity.Burst;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public sealed class JsonSaver : ISaveSystem
    {
        public string FileExtension { get { return ".json"; } }

        private readonly JsonSerializerSettings options = new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
        };

        public void Save<T>(T input, string path)
        {
            string json = JsonConvert.SerializeObject(input, options);

            File.WriteAllText(path, string.Empty);
            File.WriteAllText(path, json);
        }
        public T Load<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json, options);
        }
    }
}
