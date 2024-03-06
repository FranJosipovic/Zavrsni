using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Zavrsni.TeamOps.Features.Users.Utils
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, byte[]? salt = null)
        {
            bool isNewSaltGenerated = false;
            if (salt == null)
            {
                salt = RandomNumberGenerator.GetBytes(128 / 8);
                isNewSaltGenerated = true;
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return isNewSaltGenerated ? $"{Convert.ToBase64String(salt)}:{hashed}" : hashed;
        }
    }
}
