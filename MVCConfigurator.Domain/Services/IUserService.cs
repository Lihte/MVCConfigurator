using MVCConfigurator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MVCConfigurator.Domain.Services
    {
    public interface IUserService
        {
        Response<User> Login(string username, string password);
        Response<User> RegisterUser(string username, string password, string confirmPassword);
        //User AuthenticateRequest(HttpContextBase httpContext);
        Response<User> Get(string userName);

        }
    }
