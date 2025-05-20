using System.Text.Json;

namespace DiskIndex.Models
{
    public static class RootFolderCheck
    {
        private static Dictionary<string, string> _rootFolder;

        //Unused for now, it will serve as a method to set the root folder to the JSON file for the first setup.
        public static void SetRootFolderPath(string path)
        {
            _rootFolder = new Dictionary<string, string>{{ "rootFolderPath", path }};

            var json = JsonSerializer.Serialize(_rootFolder, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("Configs\\rootFolderPath.json", json);
        }

        // This method is used to get the root folder path from the JSON file.
        public static string GetRootFolderPath()
        {
            try
            {
                var rootFolderJSON = File.ReadAllText("Configs\\rootFolderPath.json");
                _rootFolder = JsonSerializer.Deserialize<Dictionary<string, string>>(rootFolderJSON)
                             ?? new Dictionary<string, string>();

                Console.WriteLine("Root folder path loaded from JSON: " + _rootFolder["rootFolderPath"]);

                return _rootFolder["rootFolderPath"];
            }
            catch (Exception)
            {
                Console.WriteLine("Error loading root folder path from JSON. Please check the file.");
                return string.Empty;
            }
        }

    }
}
