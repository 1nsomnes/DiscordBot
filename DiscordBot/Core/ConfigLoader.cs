using System;
using System.IO;
using Newtonsoft.Json;

namespace DiscordBot.Core
{
    public static class ConfigLoader
    {
        public static string Token { get; private set; } = "";
        public static string Prefix { get; private set; } = "";

        public static string path;

        public static void Load()
        {
            path = Directory.GetCurrentDirectory() + "/config.json";
            Console.WriteLine($"Path: {path}");
            if(!File.Exists(path))
            {
                Console.WriteLine("Config file does not exist. Creating config...");
                SaveData(null);
                return;
            }
            JSONWrapper wrapper = LoadData();

            if (wrapper.data.prefix.Equals("") || wrapper.data.token.Equals("")) return;
            Token = wrapper.data.token;
            Prefix = wrapper.data.prefix;
        }

        public static void SaveData(JSONWrapper w)
        {
            string content = JsonConvert.SerializeObject(w ?? new JSONWrapper());
            File.WriteAllText(path, content);

            if (w.data.prefix != Prefix) Prefix = w.data.prefix;
        }

        public static JSONWrapper LoadData()
        {
            if (!File.Exists(path)) return null;
            string content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<JSONWrapper>(content);
        }
    }

    [System.Serializable]
    public class JSONWrapper
    {
        public Data data = new Data();
    }

    [System.Serializable]
    public class Data
    {
        public string token = "";
        public string prefix = "";
        public AdminData adminData = new AdminData();
    }

    [System.Serializable]
    public class AdminData
    {
        public ulong logChannelId;
        public bool isLogging = false;
        public LogSettings logSettings = new LogSettings();
    }

    [System.Serializable]
    public class LogSettings
    {
        public bool editedMessage = true;
        public bool deletedMessage = true;
    }

}
