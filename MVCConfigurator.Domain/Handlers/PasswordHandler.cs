using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MVCConfigurator.Domain.Handlers
{
    public class PasswordHandler:IPasswordHandler
    {
        public void SaltAndHash(string password, out byte[] salt, out byte[] hash)
        {
            var rng = new RNGCryptoServiceProvider();
            salt = new byte[10];
            rng.GetBytes(salt);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = 1000;

            hash = pbkdf2.GetBytes(10);
        }

        public bool Validate(string password, byte[] storedSalt, byte[] storedHash)
        {
            byte[] salt = storedSalt;
            byte[] hash = storedHash;
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password,salt, 1000);


            byte[] testHash = pbkdf2.GetBytes(hash.Length);

            return SlowEquals(hash, testHash);
        }
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length^(uint)b.Length;
            for(int i = 0; i < a.Length && i<b.Length; i++)
            {
                diff |=(uint)(a[i]^b[i]);
            }
            return diff==0;
        }
    }
}
