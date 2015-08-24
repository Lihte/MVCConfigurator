using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Handlers
    {
    public interface IPasswordHandler
        {
        void SaltAndHash(string password, out byte[] salt, out byte[] hash);
        bool Validate(string password, byte[] storedSalt, byte[] storedHash);
        }
    }
