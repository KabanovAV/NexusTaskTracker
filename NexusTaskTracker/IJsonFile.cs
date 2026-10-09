namespace NexusTaskTracker
{
    public interface IJsonFile
    {
        string ReadJsonText(string path);
        void WriteJsonText(string path, Root root);
        Root DeserializeJson(string json);
    }
}
