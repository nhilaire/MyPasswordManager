using System.Security.Cryptography;
using System.Text;

namespace MyPasswordManager.Core.ApplicationServices
{
    public class CryptoService
    {
        private const int iterationCount = 10000;

        public string Encrypt(string clearText, string encryptionKey, string saltText)
        {
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            var salt = Encoding.UTF8.GetBytes(saltText);
            using var encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey, salt, iterationCount, HashAlgorithmName.SHA512);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
            clearText = Convert.ToBase64String(ms.ToArray());
            return clearText;
        }

        public string Decrypt(string cipherText, string encryptionKey, string saltText)
        {
            var salt = Encoding.UTF8.GetBytes(saltText);
            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            using var encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(encryptionKey, salt, iterationCount, HashAlgorithmName.SHA512);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
            cipherText = Encoding.Unicode.GetString(ms.ToArray());
            return cipherText;
        }
    }
}
