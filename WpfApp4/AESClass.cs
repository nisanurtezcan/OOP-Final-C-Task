using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using static WpfApp4.BruteForceClass;
using static WpfApp4.PassSaltClass;
#pragma warning disable SYSLIB0041

namespace WpfApp4
{
    public class AESClass
    {
        static string PadSalt(string salt, int length)
        {
            return salt.PadRight(length, '0');
        }

        public static string EncryptString_Aes(string plainText, string password, string salt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000);
                aesAlg.Key = key.GetBytes(32); // AES-256 key size
                aesAlg.IV = key.GetBytes(16); // AES block size

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        return Convert.ToBase64String(encrypted);
                    }
                }
            }
        }

        public static string DecryptString_Aes(string cipherText, string password, string salt)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000); // Generate key from password and salt
                    aesAlg.Key = key.GetBytes(32); // AES-256 key size
                    aesAlg.IV = key.GetBytes(16); // AES block size

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                // Decryption failed
                return "decryption_failed";
            }
        }
    }
}
