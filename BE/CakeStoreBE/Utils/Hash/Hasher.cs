using System.Security.Cryptography;
using System.Text;

namespace CakeStoreBE.Utils.Hash
{
    public static class Hasher
    {
        public static string HashWithSHA256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Phương thức hash với HMACSHA256 và khóa bí mật
        public static string HashWithHMACSHA256(string input, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = hmac.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
