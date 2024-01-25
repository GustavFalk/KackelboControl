using System.Security.Cryptography;
using KackelboControl_API.Authentication;

namespace KackelboControl_API.Helpers;

public static class Hasher
{

    public static string GetHash(string key)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(key + AuthProperties.Salt);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", string.Empty);

            return hash;
        }
    }
}
