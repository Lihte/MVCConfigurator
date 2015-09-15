using System;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ValidateAntiForgeryTokenAttribute _antiForgeryValidator;
        
        public BaseController()
        {
            _antiForgeryValidator = new ValidateAntiForgeryTokenAttribute();  
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
    }
}