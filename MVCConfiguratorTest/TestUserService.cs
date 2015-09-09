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
        //UserService service = new UserService(new FakeUserRepository(), new PasswordHandler());
        //[TestMethod]
        //public void AssertThatRegisterCustomerReturnsUserWithHashAndSalt()
        //{
        //    var response = service.RegisterUser("Tomas", "Lihte","Lihte");
        //    Assert.IsNotNull(response.Entity.Hash);
        //}
        //[TestMethod]
        //public void AssertThatRegisteredUserCanLogin()
        //{
        //    var user = service.RegisterUser("Fredrik", "Fittbög","Fittbög");
        //    Assert.IsTrue(service.Login("Fredrik", "Fittbög").Success);
        //}
    }
}
