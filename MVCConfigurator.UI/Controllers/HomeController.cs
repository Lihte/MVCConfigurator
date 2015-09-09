using MVCConfigurator.Domain.Models;
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

        private static User CurrentUser;

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

                CurrentUser = HttpContext.User as User;

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
            return View("~/Views/Admin/CreateProduct.cshtml");
        }

        [CustomAuthAttribute]
        public ActionResult ProductDetails()
        {
            return View("~/Views/Admin/ProductDetails.cshtml");
        }

        [CustomAuthAttribute]
        public ActionResult CustomerList()
        {
            return View("~/Views/Admin/CustomerList.cshtml");
        }
        #endregion

        #region Customer
        [Authorize]
        public ActionResult CustomerDetails()
        {
            return View("~/Views/Admin/CustomerDetails.cshtml");
        }

        [Authorize]
        public ActionResult SelectParts()
        {
            return View("~/Views/User/SelectParts.cshtml");
        }

        [Authorize]
        public ActionResult ConfirmOrder()
        {
            return View("~/Views/User/ConfirmOrder.cshtml");
        }

        [Authorize]
        public ActionResult OrderList()
        {
            return View("~/Views/User/OrderList.cshtml");
        }

        [Authorize]
        public ActionResult Profile()
        {
            return View("~/Views/User/Profile.cshtml");
        }

        #endregion

        public ActionResult ProductList()
        {
            if(CurrentUser.IsAdmin)
            {
                return View("~/Views/Admin/AdminProductList.cshtml");
            }

            return View("~/Views/User/ProductList.cshtml");
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
        public ActionResult CreateUser(RegisterViewModel viewModel)
        {
            var userDetails = new UserDetails()
            {
                FirstName = viewModel.UserDetails.FirstName,
                LastName = viewModel.UserDetails.LastName,
                Company = viewModel.UserDetails.Company,
                Address = viewModel.UserDetails.Address,
                Phone = viewModel.UserDetails.Phone
            };

            var response = _userService.RegisterUser(viewModel.Username,viewModel.Password,viewModel.ConfirmPassword, userDetails);
            
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