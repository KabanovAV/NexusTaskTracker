using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace NexusTaskTracker
{
    public class JsonFile : IJsonFile
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

        public string ReadJsonText(string path)
            => File.ReadAllText(path);

        public void WriteJsonText(string path, Root root)
            => File.WriteAllText(path, SerializeJson(root));

        public Root DeserializeJson(string json)
            => JsonSerializer.Deserialize<Root>(json, Options) ?? new Root();

        private string SerializeJson(Root root)
            => JsonSerializer.Serialize(root, Options);
    }
}
