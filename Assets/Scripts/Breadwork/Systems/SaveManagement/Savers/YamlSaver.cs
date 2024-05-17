using System.IO;
using Unity.Burst;
using YamlDotNet.Serialization;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public sealed class YamlSaver : ISaveSystem
    {
        public string FileExtension { get { return ".yml"; } } // Unity starts throwing some errors when using .yaml

        public void Save<T>(T input, string path)
        {
            string yaml = new Serializer().Serialize(input);
            File.WriteAllText(path, string.Empty);
            File.WriteAllText(path, yaml);
        }
        public T Load<T>(string path)
        {
            string yaml = File.ReadAllText(path);

            T loadedStruct = new Deserializer().Deserialize<T>(yaml);
            return loadedStruct;
        }
    }
}
