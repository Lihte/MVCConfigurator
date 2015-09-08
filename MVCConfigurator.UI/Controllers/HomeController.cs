using MVCConfigurator.Domain.Services;
using MVCConfigurator.UI.Models;
using MVCConfigurator.UI.Security;
using MVCConfigurator.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IUserService userService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel)
        {
            var login = _userService.Login(viewModel.Username,viewModel.Password);

            if(login.Success)
            {
                _authenticationService.LoginUser(login.Entity, HttpContext, false);

                if(login.Entity.IsAdmin)
                {
                    return RedirectToAction("Admin");
                }

                return RedirectToAction("User");
            }

            ModelState.AddModelError("username",login.Error.ToString());
            return View();
        }

        #region Admin
        [CustomAuthAttribute]
        public ActionResult CreateProduct()
        {
            return View("~/Views/Home/Admin/CreateProduct.cshtml");
        }

        [CustomAuthAttribute]
        public ActionResult ProductDetails()
        {
            return View("~/Views/Home/Admin/ProductDetails.cshtml");
        }

        [CustomAuthAttribute]
        public ActionResult CustomerList()
        {
            return View("~/Views/Home/Admin/CustomerList.cshtml");
        }
        #endregion

        #region Customer

        public ActionResult CustomerDetails()
        {
            return View("~/Views/Home/Admin/CustomerDetails.cshtml");
        }

        #endregion

        public ActionResult ProductList()
        {
            var user = HttpContext.User;

            if(user.IsInRole(""))
            {
                return View("~/Views/Home/Admin/AdminProductList.cshtml");
            }

            return View("~/Views/Home/User/ProductList.cshtml");
        }

        public ActionResult SelectParts()
        {
            return View("~/Views/Home/User/SelectProducts.cshtml");
        }

        public ActionResult ConfirmOrder()
        {
            return View("~/Views/Home/User/ConfirmOrder.cshtml");
        }

        public ActionResult OrderList()
        {
            return View("~/Views/Home/User/OrderList.cshtml");
        }

        public ActionResult Profile()
        {
            return View("~/Views/Home/User/Profile.cshtml");
        }

        [Authorize]
        public ActionResult User()
        {
            return View();
        }

        [CustomAuthAttribute]
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserViewModel viewModel)
        {
            var response = _userService.RegisterUser(viewModel.Username,viewModel.Password,viewModel.ConfirmPassword);
            
            if(response.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("username", response.Error.ToString());
            
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(UserViewModel user)
        {
            _authenticationService.ResetPassword(user.Username);

            return RedirectToAction("Index");
        }

        public ActionResult CreateNewPassword(string token)
        {
            return View();
        }

        public ActionResult LogOut()
        {
            _authenticationService.LogOut();

            return RedirectToAction("Index");
        }

        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            if(filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.AuthenticationType == AuthenticationMode.Forms.ToString())
            {
                _authenticationService.AuthenticateRequest(filterContext.HttpContext);
            }
        }
    }
}