using System.Text.Json;

namespace ReUnited_Backend
{
    public class Utils
    {
        public static List<T> GetFileContent<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static string ReSerialize<T>(List<T> input)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            return JsonSerializer.Serialize(input, options);
        }

        public static void WriteFileContent(string filePath, string jsonFile)
        {
            File.WriteAllText(filePath, jsonFile);
        }

    }
}
