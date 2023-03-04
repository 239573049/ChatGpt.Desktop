using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ChatGPT.Helper;

public static class DESHelper
{
    private static readonly byte[] Key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
    private static readonly byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

    public static string Encrypt(string plainText)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
        using (var des = new DESCryptoServiceProvider())
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] inputBytes = Convert.FromBase64String(cipherText);
        using (var des = new DESCryptoServiceProvider())
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}