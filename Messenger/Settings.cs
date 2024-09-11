using Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messenger
{
    public static class Settings
    {
        #region class
        public class SettingsInfo
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string IP { get; set; }
            public List<string> AdressPort { get; set; }
            public bool isHashing { get; set; }

            public SettingsInfo(string Username,string Password, string IP, List<string> AdressPort, bool isHashing)
            {
                this.Username = Username;
                this.Password = Password;
                this.IP = IP;
                this.AdressPort = AdressPort;
                this.isHashing = isHashing;
            }
        }

        #endregion
        #region Variables

        public static string Username = "";
        public static string Password = "";
        public static string IP = "127.0.0.1";
        public static List<string> adressPort = new List<string>();
        public static bool isHashing = false;
        private static string Key = "";
        #endregion
        #region Methods
        public static SettingsInfo GetSettingsInfo()
        {
            return new SettingsInfo(Username, Password, IP, adressPort, isHashing);
        }

        public static void UpdateSettingsFromSettingsFile(SettingsInfo info)
        {
            if (info.isHashing)
            {
                Username = info.Username;
                IP = info.IP;
                adressPort = info.AdressPort;
                isHashing = info.isHashing;
            }
            else
            {
                Username = info.Username;
            }




            
        }

        public static bool ReadFileSettings()
        {
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.settings");

            if (!File.Exists(settingsFilePath))
            {
                using (StreamWriter writer = new StreamWriter(settingsFilePath))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(GetSettingsInfo()));
                }
                return false;
            }
            else
            {
                string fileSettings = File.ReadAllText(settingsFilePath);
                UpdateSettingsFromSettingsFile(JsonConvert.DeserializeObject<SettingsInfo>(fileSettings));
                return true;
            }
        }

        #endregion
    }

}
