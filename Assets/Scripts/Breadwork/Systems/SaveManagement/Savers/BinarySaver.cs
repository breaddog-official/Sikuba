using System.IO;
using Unity.Burst;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public sealed class BinarySaver : ISaveSystem
    {
        public string FileExtension { get { return ".dat"; } }

        public void Save<T>(T input, string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            new BinaryFormatter().Serialize(fs, input);
        }
        public T Load<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);

            return (T)new BinaryFormatter().Deserialize(fs);
        }
    }
}

