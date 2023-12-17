using System.IO;
using Scripts.Core;
using Unity.Burst;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public sealed class WithoutSerializationSaver : ISaveSystem
    {
        public string FileExtension { get; }
        public WithoutSerializationSaver(string extension = ".txt")
        {
            FileExtension = extension;
        }

        public void Save<T>(T input, string path)
        {
            File.WriteAllText(path, string.Empty);
            File.WriteAllText(path, input.ToString());
        }
        public T Load<T>(string path)
        {
            string text = File.ReadAllText(path);
            return GameManager.AdvancedConvert<T>(text);
        }
    }
}
