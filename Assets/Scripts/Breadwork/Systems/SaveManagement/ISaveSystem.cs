namespace Scripts.SaveManagement
{
    public interface ISaveSystem
    {
        string FileExtension { get; }

        void Save<T>(T value, string path);
        T Load<T>(string path);
    }
}
