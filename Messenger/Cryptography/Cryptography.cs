using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Messenger.Cryptography
{
    public class Cryptography
    {
        private static readonly byte[] DefaultKey = Encoding.UTF8.GetBytes("MyDefaultKey1234"); // 16 байт для AES-128
        private static readonly byte[] DefaultIV = new byte[16]; // IV размером 16 байт (для AES)

        private static Random random = new Random();
        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public static string Encrypt(string Text, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(Text);
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string Decrypt(string Text, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(Text)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        public static string DefaultEncrypt(string text)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = DefaultKey;
                aesAlg.GenerateIV(); // Генерация случайного IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Сначала записываем IV
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string DefaultDecrypt(string encryptedText)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = DefaultKey;

                // Извлекаем IV из первых 16 байт
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        public static string RandomKeySha256Hash()
        {

            byte[] textBytes = Encoding.UTF8.GetBytes(GenerateRandomString(random.Next(101)));
            byte[] keyBytes = Encoding.UTF8.GetBytes(GenerateRandomString(random.Next(101)));

            using (HMACSHA256 hmacSha256 = new HMACSHA256(keyBytes))
            {

                byte[] hashBytes = hmacSha256.ComputeHash(textBytes);


                StringBuilder hash = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hash.Append(b.ToString("x1"));
                }
                return hash.ToString();
            }
        }

        public static string EncryptSHA512(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            using (HMACSHA512 hmacSha512 = new HMACSHA512(textBytes))
            {
                byte[] hashBytes = hmacSha512.ComputeHash(textBytes);

                StringBuilder hash = new StringBuilder();
                foreach(byte b in hashBytes)
                {
                    hash.Append(b.ToString("x1"));
                }
                return hash.ToString();
            }
        }
    }
}
