using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.DAL.Repositories;
using MVCConfigurator.DAL.Handlers;

namespace MVCConfiguratorTest
{
    [TestClass]
    public class TestUserService
    {
        CustomAuthenticationService service = new CustomAuthenticationService(new FakeUserRepository(), new PasswordHandler());
        [TestMethod]
        public void AssertThatRegisterCustomerReturnsUserWithHashAndSalt()
        {
            var user = service.RegisterUser("Tomas", "Lihte");
            Assert.IsNotNull(user.Hash);
        }
        [TestMethod]
        public void AssertThatRegisteredUserCanLogin()
        {
            var user = service.RegisterUser("Fredrik", "Fittbög");
            Assert.IsTrue(service.Login("Fredrik", "Fittbög"));
        }
    }
}
