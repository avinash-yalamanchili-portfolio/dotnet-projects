// Services/EncryptionService.cs
using System.Security.Cryptography;
using System.Text;

namespace MemberAPI.Services
{
    public class EncryptionService
    {
        private readonly byte[] key;

        public EncryptionService(IConfiguration config)
        {
            // Store your 32-byte encryption key securely (e.g. Azure Key Vault)
            var keyString = config["EncryptionKey"];
            if (string.IsNullOrEmpty(keyString) || keyString.Length != 32)
                throw new Exception("Encryption key must be 32 characters");

            key = Encoding.UTF8.GetBytes(keyString);
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor();

            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            var bytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = key;

            var iv = new byte[16];
            Array.Copy(bytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();

            using var ms = new MemoryStream(bytes, 16, bytes.Length - 16);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
