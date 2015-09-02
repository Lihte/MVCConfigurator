using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.Domain.Repositories;
using MVCConfigurator.DAL.Repositories;
using MVCConfigurator.DAL.Handlers;
using MVCConfigurator.Domain.Handlers;
using MVCConfigurator.UI.Services;

namespace MVCConfigurator.UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IProductRepository, FakeProductRepository>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IUserRepository, FakeUserRepository>();
            container.RegisterType<IOrderRepository, FakeOrderRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IPasswordHandler, PasswordHandler>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IMessageService, MessageService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            
        }
    }
}