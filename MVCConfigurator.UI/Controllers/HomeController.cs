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
using System.Web.Mvc.Filters;

namespace MVCConfigurator.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        private static User CurrentUser;

        public HomeController(IUserService userService, IProductService productService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _productService = productService;
            _authenticationService = authenticationService;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel)
        {
            var login = _userService.Login(viewModel.Username, viewModel.Password);

            if (login.Success)
            {
                _authenticationService.LoginUser(login.Entity, HttpContext, false);

                CurrentUser = HttpContext.User as User;

                if (login.Entity.IsAdmin)
                {
                    return RedirectToAction("Admin");
                }

                return RedirectToAction("User");
            }

            ModelState.AddModelError("username", login.Error.ToString());
            return View();
        }

        #region Admin
        //[CustomAuthAttribute]
        public ActionResult CreateProduct()
        {
            var viewModel = new ProductViewModel();
            viewModel.Categories = _productService.GetAllProductCategories().Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name });

            return View("~/Views/Admin/CreateProduct.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel model)
        {
            var product = new Product()
            {
                Name = model.Product.Category,
                Category = new ProductCategory { Name = model.Product.Category },
                ImagePath = model.Product.ImagePath
            };
            product = _productService.AddProduct(product);

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }
        public ActionResult AddPart(int id)
        {
            var viewModel = new PartViewModel();
            viewModel.Categories = _productService.GetAllPartCategoriesByProduct(_productService.GetProduct(id)).Select(
                c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name });
            viewModel.ProductId = id;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddPart(PartViewModel model)
        {
            var part = new Part()
            {
                Category = model.PartDetails.Category,
                ImagePath = model.PartDetails.ImagePath,
                LeadTime = model.PartDetails.LeadTime,
                Name = model.PartDetails.Name,
                Price = model.PartDetails.Price,
                StockKeepingUnit = model.PartDetails.StockKeepingUnit
            };

            var product = _productService.GetProduct(model.ProductId);
            product.Parts.Add(part);
            _productService.UpdateProduct(product);

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }
        public ActionResult ProductPartList(int id)
        {
            var product = _productService.GetProduct(id);
            var viewModel = new ProductViewModel
            {
                Product = new ProductModel
                {
                    Id = id,
                    Category = product.Name,
                    Parts = product.Parts,
                    ImagePath = product.ImagePath,
                    ProductCode = product.ProductCode
                },
            };
            return View(viewModel);
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
            if (CurrentUser.IsAdmin)
            {
                var viewModel = new ProductListViewModel
                {
                    Products = _productService.GetAllProducts()
                };

                return View("~/Views/Admin/AdminProductList.cshtml", viewModel);
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

            var response = _userService.RegisterUser(viewModel.Username, viewModel.Password, viewModel.ConfirmPassword, userDetails);

            if (response.Success)
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

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.AuthenticationType == AuthenticationMode.Forms.ToString())
            {
                _authenticationService.AuthenticateRequest(filterContext.HttpContext);
            }
        }
    }
}