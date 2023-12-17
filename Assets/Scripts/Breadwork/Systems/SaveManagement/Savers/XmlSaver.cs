using System.IO;
using Unity.Burst;
using System.Xml.Serialization;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public sealed class XmlSaver : ISaveSystem
    {
        public string FileExtension { get { return ".xml"; } }

        public void Save<T>(T input, string path)
        {
            File.WriteAllText(path, string.Empty);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            new XmlSerializer(typeof(T))
                .Serialize(fs, input);
        }
        public T Load<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);

            return (T)new XmlSerializer(typeof(T))
                .Deserialize(fs);
        }
    }
}

