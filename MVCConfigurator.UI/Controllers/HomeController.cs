using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.UI.Models;
using MVCConfigurator.UI.Security;
using MVCConfigurator.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;


namespace MVCConfigurator.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public User CurrentUser { get { return System.Web.HttpContext.Current.User as User; } }

        public HomeController(IUserService userService,
            IProductService productService, IOrderService orderService, IAuthenticationService authenticationService) : base(authenticationService)
        {
            if (userService == null)
                throw new ArgumentNullException("userService", new Exception("The dependency injection for userService has failed for some reason."));
            if (productService == null)
                throw new ArgumentNullException("productService", new Exception("The dependency injection for productService has failed for some reason."));
            if (orderService == null)
                throw new ArgumentNullException("orderService", new Exception("The dependency injection for orderService has failed for some reason."));

            _userService = userService;
            _productService = productService;
            _orderService = orderService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var viewModel = new FeaturedProductViewModel();

            viewModel.FeaturedProducts = _productService.GetAllProducts().OrderByDescending(p => p.Id).Take(3).Select(p => new ProductModel(p)).ToList();

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Customer()
        {
            var viewModel = new FeaturedProductViewModel();

            viewModel.FeaturedProducts = _productService.GetAllProducts().OrderByDescending(p => p.Id).Take(3).Select(p => new ProductModel(p)).ToList();

            return View(viewModel);
        }

        [Authorize]
        public ActionResult CustomizeProduct()
        {
            var viewModel = new CustomizeProductViewModel { Products = _productService.GetAllProducts().Select(p => new ProductModel(p)).ToList() };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult SelectParts(int id)
        {
            var product = _productService.GetProduct(id);

            var viewModel = new CustomizeProductViewModel()
            {
                Product = new ProductModel(product)
            };

            viewModel.Product.Parts = product.Parts.Select(p => new PartModel(p)).OrderBy(m => m.CategoryId).ToList();

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SelectParts(CustomizeProductViewModel viewModel)
        {
            var selectedPartList = viewModel.Product.Parts.Where(p => p.IsSelected == true).ToList();

            var totalCat = viewModel.Product.Parts.Select(p => p.CategoryId).Distinct().ToArray();

            // Setup so that one part of every category must be selected before completion
            if (selectedPartList.Count() != totalCat.Count())
            {
                return View(viewModel);
            }

            var partList = new List<Part>();

            foreach (var item in selectedPartList)
            {
                partList.Add(_productService.GetProduct(viewModel.Product.Id).Parts.SingleOrDefault(p => p.Id == item.Id));
            }

            if (partList != null || partList.Count > 0)
            {
                double orderLeadtime = new double();

                foreach (var item in partList)
                {
                    orderLeadtime += item.LeadTime;
                }

                StringBuilder productCode = new StringBuilder();

                var order = new Order()
                {
                    Product = _productService.GetProduct(viewModel.Product.Id),
                    DeliveryDate = DateTime.Now.AddDays(orderLeadtime),
                    IsReady = false,
                    User = _userService.Get(HttpContext.User.Identity.Name).Entity
                };

                foreach (var part in partList)
                {
                    order.Price += part.Price;
                    order.IsReady = order.DeliveryDate > DateTime.Now ? false : true;
                    productCode.Append(part.StockKeepingUnit);
                }

                order.ProductCode = productCode.ToString();

                _orderService.AddOrder(order);
            }

            return RedirectToAction("OrderList");
        }

        [Authorize]
        [HttpGet]
        public JsonResult GetIncompatibleParts(int productId, int partId)
        {
            var product = _productService.GetProduct(productId);

            var ip = product.Parts.SingleOrDefault(p => p.Id == partId).IncompatibleParts.Select(p => p.Id);

            return Json(new { ip = ip }, JsonRequestBehavior.AllowGet);
        }

        //[Authorize]
        //public ActionResult ConfirmOrder(Order order)
        //{
        //    var viewModel = new OrderViewModel(order);

        //    return View("~/Views/User/ConfirmOrder.cshtml", viewModel);
        //}

        //public ActionResult ConfirmOrder(OrderViewModel order)
        //{
        //    var viewModel = new OrderViewModel(order);

        //    return View("~/Views/User/ConfirmOrder.cshtml", viewModel);
        //}

        [Authorize]
        public ActionResult OrderList()
        {
            var orders = _orderService.GetOrdersByCustomer(CurrentUser.Id);
            var viewModel = new CustomerDetailsViewModel()
            {
                Orders = orders.Select(o => new OrderModel { Id = o.Id, DeliveryDate = o.DeliveryDate, IsReady = o.IsReady }).ToList(),
                Customer = CurrentUser
            };

            return View(viewModel);
        }

        #region AccountAndLogin
        public ActionResult CreateUser()
        {
            return View("~/Views/Home/LogIn/CreateUser.cshtml");
        }

        [HttpPost]
        public ActionResult CreateUser(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/LogIn/CreateUser.cshtml");
            }

            var userDetails = new UserDetails()
            {
                FirstName = viewModel.UserDetails.FirstName,
                LastName = viewModel.UserDetails.LastName,
                Company = viewModel.UserDetails.Company,
                Address = viewModel.UserDetails.Address,
                Phone = viewModel.UserDetails.Phone
            };

            var response = _userService.RegisterUser(viewModel.Username,
                viewModel.Password, viewModel.ConfirmPassword, userDetails);

            if (response.Success)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("username", response.Error.ToString());

            return View("~/Views/Home/LogIn/CreateUser.cshtml");
        }

        public ActionResult ResetPassword()
        {
            return View("~/Views/Home/LogIn/ResetPassword.cshtml");
        }

        [HttpPost]
        public ActionResult ResetPassword(UserViewModel user)
        {
            _authenticationService.ResetPassword(user.Username);
            return RedirectToAction("Index");
        }

        public ActionResult CreateNewPassword(string token)
        {
            return View("~/Views/Home/LogIn/CreateNewPassword.cshtml");
        }

        public ActionResult LogIn()
        {
            return View("~/Views/Home/LogIn/LogIn.cshtml");
        }

        [HttpPost]
        public ActionResult LogIn(UserViewModel viewModel)
        {
            var login = _userService.Login(viewModel.Username, viewModel.Password);

            if (login.Success)
            {
                _authenticationService.LoginUser(login.Entity, HttpContext, false);

                if (login.Entity.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Customer", "Home");
            }

            ModelState.AddModelError("username", login.Error.ToString());
            return View();
        }

        public ActionResult LogOut()
        {
            _authenticationService.LogOut();
            return RedirectToAction("Index");
        } 
        #endregion  
    }
}