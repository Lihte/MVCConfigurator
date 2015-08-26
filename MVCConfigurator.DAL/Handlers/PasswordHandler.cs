using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MVCConfigurator.Domain.Handlers;

namespace MVCConfigurator.DAL.Handlers
{
    public class PasswordHandler : IPasswordHandler
    {
        private const int SALT_SIZE = 10;
        private const int HASH_SIZE = 10;
        private const int ITERATIONS = 1000;

        public void SaltAndHash(string password, out byte[] salt, out byte[] hash)
        {
            var rng = new RNGCryptoServiceProvider();
            salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = ITERATIONS;

            hash = pbkdf2.GetBytes(HASH_SIZE);
        }

        public bool Validate(string password, byte[] storedSalt, byte[] storedHash)
        {
            byte[] salt = storedSalt;
            byte[] hash = storedHash;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);

            byte[] testHash = pbkdf2.GetBytes(hash.Length);

            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length^(uint)b.Length;
            for(int i = 0; i < a.Length && i<b.Length; i++)
            {
                diff |= (uint)(a[i]^b[i]);
            }
            return diff == 0;
        }
    }
}
