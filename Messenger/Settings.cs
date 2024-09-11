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
using Messenger.Cryptography;

namespace Messenger
{
    public static class Settings
    {
        #region Enum
        public enum EncryptionMethod
        {
            None,
            Encrypt,
            Encrypt_Local
        }
        #endregion
        #region Class
        public class SettingsInfo
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string IP { get; set; }
            public List<int> AdressPort { get; set; }
            public EncryptionMethod Hashing { get; set; }

            public SettingsInfo(string Username,string Password, string IP, List<int> AdressPort, EncryptionMethod Hashing)
            {
                this.Username = Username;
                this.Password = Password;
                this.IP = IP;
                this.AdressPort = AdressPort;
                this.Hashing = Hashing;
            }
        }

        #endregion
        #region Variables

        public static string Username = "";
        public static string Password = "";
        public static string IP = "127.0.0.1";
        public static List<int> adressPort = new List<int> 
        {
            10000
        };
        public static EncryptionMethod Hashing = EncryptionMethod.None;
        private static string Key = "";
        #endregion
        #region Methods
        public static SettingsInfo GetSettingsInfo()
        {
            return new SettingsInfo(Username, Password, IP, adressPort, Hashing);
        }

        public static void UpdateSettingsFromSettingsFile(SettingsInfo info)
        {
            if (info != null && info.Username != "" && info.Password != "")
            {
                if (info.Hashing != EncryptionMethod.None)
                {
                    Username = info.Username;
                    Password = info.Password;
                    IP = info.IP;
                    if (info.AdressPort.Count == 0) adressPort = info.AdressPort;
                    Hashing = info.Hashing;
                }
                else
                {
                    Username = Cryptography.Cryptography.DefaultEncrypt(info.Username);
                    Password = Cryptography.Cryptography.DefaultEncrypt(info.Password);
                    IP = info.IP;
                    if (info.AdressPort.Count == 0) adressPort = info.AdressPort;
                    Hashing = EncryptionMethod.Encrypt_Local;

                    UpdateSettingsInSettingsFile(GetSettingsInfo());
                }
            }
            else 
            {
                UpdateSettingsInSettingsFile(GetSettingsInfo());
            }
        }

        public static void UpdateSettingsInSettingsFile(SettingsInfo info)
        {
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.settings");

            using (StreamWriter writer = new StreamWriter(settingsFilePath))
            {
                writer.WriteLine($"{JsonConvert.SerializeObject(GetSettingsInfo())}");
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
