using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.More
{
    public class clsSecurity
    {
        static private string GetHashedPassword(string salt, string password)
        {
            byte[] Salt = Convert.FromBase64String(salt);
            byte[] hash;
            // نستخدم 310,000 تكرار مع SHA256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Salt, 310000, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32); // 256 بت
            }
            return Convert.ToBase64String(hash);
        }

        static public string CreateNewHashPassword(string password)
        {
            
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 310000, HashAlgorithmName.SHA256))
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
            if(SplitHashRecord.Length != 2) return false;
            string s = SplitHashRecord[0];
            string h = SplitHashRecord[1];
                
            if (h.Equals(GetHashedPassword(s, password))) return true ;
            return false;
        }

        public static string SimpleEncrypt(string password, string key)
        {
            StringBuilder encrypted = new StringBuilder();
            
            for (int i = 0; i < password.Length; i++)
            {
                // تطبيق XOR بين حرف كلمة السر وحرف المفتاح
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
                // تحويل القيمة السداسية عشرية إلى حرف
                int charCode = Convert.ToInt32(encrypted.Substring(i, 2), 16);
                // تطبيق XOR مرة أخرى لفك التشفير
                char decryptedChar = (char)(charCode ^ key[(i / 2) % key.Length]);
                decrypted.Append(decryptedChar);
            }
            
            return decrypted.ToString();
        }

    }

}
