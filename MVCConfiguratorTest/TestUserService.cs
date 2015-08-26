﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.DAL.Repositories;
using MVCConfigurator.DAL.Handlers;
using MVCConfigurator.UI.Authentication;

namespace MVCConfiguratorTest
{
    [TestClass]
    public class TestUserService
    {
        CustomAuthenticationService service = new CustomAuthenticationService(new FakeUserRepository(), new PasswordHandler(),new FormsAuthenticationAdapter());
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
