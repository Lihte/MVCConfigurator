using MVCConfigurator.UI.Services;
using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MVCConfigurator.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ValidateAntiForgeryTokenAttribute _antiForgeryValidator;

        protected readonly IAuthenticationService _authenticationService;

        public BaseController(IAuthenticationService authenticationService)
        {
            if (authenticationService == null)
                throw new ArgumentNullException("authenticationService", new Exception("The dependency injection for authenticationService has failed for some reason."));

            _antiForgeryValidator = new ValidateAntiForgeryTokenAttribute();
            _authenticationService = authenticationService;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.GetHttpMethodOverride()
                .Equals("post", StringComparison.CurrentCultureIgnoreCase))
            {
                _antiForgeryValidator.OnAuthorization(filterContext);
            }

            base.OnAuthorization(filterContext);
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.User != null && filterContext
                .HttpContext.User.Identity.AuthenticationType == AuthenticationMode.Forms.ToString())
            {
                _authenticationService.AuthenticateRequest(filterContext.HttpContext);
            }
        }
    }
}