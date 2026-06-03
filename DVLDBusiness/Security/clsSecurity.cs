using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DVLD.More
{
    public class clsSecurity
    {
        // الفلفل (Pepper): قيمة سرية وثابتة تُحفظ في السيرفر خارج قاعدة البيانات
        
        static private string RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\zDVLD";
        private static string GetHashedPassword(string salt, string password)
        {
            byte[] Salt = Convert.FromBase64String(salt);

            // دمج كلمة المرور مع الفلفل
            string passwordWithPepper = password + Registry.GetValue(RegistryPath, "Pepper", "") as string;

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordWithPepper, Salt, 310000, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32); // 256 بت
            }
            return Convert.ToBase64String(hash);
        }

        public static string CreateNewHashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // دمج كلمة المرور مع الفلفل
            string passwordWithPepper = password + Registry.GetValue(RegistryPath, "Pepper", "") as string;

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordWithPepper, salt, 310000, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32);
            }
            string saltBase64 = Convert.ToBase64String(salt);
            string hashBase64 = Convert.ToBase64String(hash);

            return saltBase64 + ":" + hashBase64;
        }

        public static bool IsTruePassword(string password, string HashedPasswordWithSalt)
        {
            string[] SplitHashRecord = HashedPasswordWithSalt.Split(':');
            if (SplitHashRecord.Length != 2) return false;

            string s = SplitHashRecord[0]; // الملح
            string h = SplitHashRecord[1]; // الهاش المخزن

            // العودة إلى طريقة المقارنة الأصلية باستخدام Equals
            string jjjj = GetHashedPassword(s, password);
            if (h.Equals(GetHashedPassword(s, password))) return true;
            return false;
        }

        #region XOR Encryption (SimpleEncrypt / SimpleDecrypt)
        public static string SimpleEncrypt(string password, string key)
        {
            StringBuilder encrypted = new StringBuilder();
            for (int i = 0; i < password.Length; i++)
            {
                char encryptedChar = (char)(password[i] ^ key[i % key.Length]);
                encrypted.Append(Convert.ToString((int)encryptedChar, 16).PadLeft(2, '0'));
            }
            return encrypted.ToString();
        }

        public static string SimpleDecrypt(string encrypted, string key)
        {
            StringBuilder decrypted = new StringBuilder();
            for (int i = 0; i < encrypted.Length; i += 2)
            {
                int charCode = Convert.ToInt32(encrypted.Substring(i, 2), 16);
                char decryptedChar = (char)(charCode ^ key[(i / 2) % key.Length]);
                decrypted.Append(decryptedChar);
            }
            return decrypted.ToString();
        }
        #endregion
    }
}