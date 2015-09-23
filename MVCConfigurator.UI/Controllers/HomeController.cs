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
using System.Web.Mvc.Filters;

namespace MVCConfigurator.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IOrderService _orderService;

        public static User CurrentUser;

        public HomeController(IUserService userService, 
            IProductService productService, IAuthenticationService authenticationService, IOrderService orderService)
        {
            _userService = userService;
            _productService = productService;
            _authenticationService = authenticationService;
            _orderService = orderService;
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

            if(login.Success)
            {
                _authenticationService.LoginUser(login.Entity, HttpContext, false);

                CurrentUser = HttpContext.User as User;

                if(login.Entity.IsAdmin)
                {
                    return RedirectToAction("Admin");
                }

                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("username", login.Error.ToString());
            return View();
        }

        #region Admin
        [Authorize]
        public ActionResult CreateProduct()
        {
            var viewModel = new ProductViewModel();
            viewModel.Categories = _productService.GetAllProductCategories()
                .Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name });

            return View("~/Views/Admin/CreateProduct.cshtml", viewModel);
        }
        
        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel model)
        {
            var path = "";
            var fullPath = "";

            var categories = _productService.GetAllProductCategories();

            if(categories.Any(c => c.Name == model.Product.Category))
            {
                return View("~/Views/Admin/CreateProduct.cshtml");
            }

            if(model.Product.Image.ImageUpload.FileName!=null)
            {
                var fileName = Path.GetFileName(model.Product.Image.ImageUpload.FileName);
                path = Url.Content(Path.Combine(Server.MapPath("~/Content/Images"), fileName));
                model.Product.Image.ImageUpload.SaveAs(path);
                fullPath = @"~/Content/Images/" + fileName;
            }

            var product = new Product()
            {
                Name = model.Product.Category,
                Category = new ProductCategory { Name = model.Product.Category },
                ImagePath = fullPath
            };
            

            product = _productService.AddProduct(product);

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }

        public ActionResult DeleteProduct(int id)
        {
            var product = _productService.GetProduct(id);

            _productService.DeleteProduct(product);

            return RedirectToAction("ProductList");
        }
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase photo)
        //{
        //    var path = "";
        //    if(photo != null && photo.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(photo.FileName);
        //        path = Url.Content(Path.Combine(Server.MapPath("~/Content/Images"), fileName));
        //        photo.SaveAs(path);
        //    }

        //    return RedirectToAction("CreateProduct");
        //}
        public ActionResult AddPart(int id)
        {
            var product = _productService.GetProduct(id);

            var viewModel = new PartViewModel()
            {
                Categories = _productService.GetAllPartCategoriesByProduct(product)
                .Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name }),
                ExistingParts = product.Parts.Select(p => new PartModel(p)).ToList(),
                ProductId = id
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddPart(PartViewModel model)
        {
            var path="";
            var fullPath= "N/A";
            var product = _productService.GetProduct(model.ProductId);

            var partCategory = product.Parts.FirstOrDefault(p => p.Category.Id == model.PartDetails.CategoryId);

            var incompatibleParts = new List<Part>();

            if(model.PartDetails.Image.PartImageUpload !=null)
            {
                var fileName = Path.GetFileName(model.PartDetails.Image.PartImageUpload.FileName);
                path = Url.Content(Path.Combine(Server.MapPath("~/Content/Images"), fileName));
                model.PartDetails.Image.PartImageUpload.SaveAs(path);
                fullPath = @"~/Content/Images/" + fileName;
            }

            if(model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach(var item in model.ExistingParts)
                {
                    if(item.IsIncompatible)
                    {
                        incompatibleParts.Add(product.Parts.First(p => p.Id == item.Id));
                    }
                }
            }

            var sameCategory = product.Parts.Where(p => p.Category.Id == model.PartDetails.CategoryId).Select(p => p);

            incompatibleParts.AddRange(sameCategory);

            var part = new Part()
            {
                Category = partCategory != null ? partCategory.Category : new PartCategory { Name = model.PartDetails.Category },
                ImagePath = fullPath,
                LeadTime = model.PartDetails.LeadTime,
                Name = model.PartDetails.Name,
                Price = model.PartDetails.Price,
                StockKeepingUnit = model.PartDetails.StockKeepingUnit,
                IncompatibleParts = incompatibleParts
            };

            product.Parts.Add(part);
            _productService.UpdateProduct(product);

            part = _productService.GetProduct(model.ProductId).Parts.LastOrDefault();

            if(model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach(var item in model.ExistingParts)
                {
                    if(item.IsIncompatible)
                    {
                        var partUpdate = product.Parts.SingleOrDefault(p => p.Id == item.Id);
                        partUpdate.IncompatibleParts.Add(part);
                    }
                }
            }

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }

        public ActionResult EditPart(int productId, int partId)
        {
            var product = _productService.GetProduct(productId);
            var part = product.Parts.SingleOrDefault(p => p.Id == partId);

            var viewModel = new PartViewModel()
            {
                ExistingParts = product.Parts.Where(p => p.Id != part.Id).Select(p => new PartModel(p) { IsIncompatible = part.IncompatibleParts.Contains(p) }).ToList(),
                ProductId = productId,
                PartDetails = new PartModel(part)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditPart(PartViewModel model)
        {
            var path = "";
            var fullPath = "N/A";
            var product = _productService.GetProduct(model.ProductId);

            var partCategory = product.Parts.FirstOrDefault(p => p.Category.Id == model.PartDetails.CategoryId);

            var incompatibleParts = new List<Part>();

            //var f  = product.Parts.Select(x => new { id = x.Id, ip = x.IncompatibleParts.Select(y => y.Id) });
            //var dict = new Dictionary<int, int>();
            //foreach (var e in f)
            //{
            //    foreach(var c in e.ip)
            //    {
            //        dict.Add(e.id, c);
            //    }
            //}

            var incompatibleParts = new List<Part>();

            if(model.PartDetails.Image.PartImageUpload != null)
            {
                var fileName = Path.GetFileName(model.PartDetails.Image.PartImageUpload.FileName);
                path = Url.Content(Path.Combine(Server.MapPath("~/Content/Images"), fileName));
                model.PartDetails.Image.PartImageUpload.SaveAs(path);
                fullPath = @"~/Content/Images/" + fileName;
            }

            if(model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach(var item in model.ExistingParts)
                {
                    if(item.IsIncompatible)
                    {
                        incompatibleParts.Add(product.Parts.First(p => p.Id == item.Id));
                    }
                }
            }

            var part = product.Parts.SingleOrDefault(m => m.Id == model.PartDetails.Id);

            _productService.UpdateProduct(product, part);

            var sameCategory = product.Parts.Where(p => (p.Category.Id == part.Category.Id) && (p.Id != part.Id)).Select(p => p);

            incompatibleParts.AddRange(sameCategory);

            part.ImagePath = fullPath;
            part.LeadTime = model.PartDetails.LeadTime;
            part.Name = model.PartDetails.Name;
            part.Price = model.PartDetails.Price;
            part.StockKeepingUnit = model.PartDetails.StockKeepingUnit;
            part.IncompatibleParts = incompatibleParts;

            foreach (var p in part.IncompatibleParts)
            {
                p.IncompatibleParts.Add(part);
            }

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
                    Parts = product.Parts.Select(p => new PartModel(p)).ToList(),
                    Image = new Image(product.ImagePath),
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
            var model = new CustomerListViewModel()
            {
                Users = _userService.GetAllUsers().Where(u => u.IsAdmin == true).ToList()
            };
            return View("~/Views/Admin/CustomerList.cshtml", model);
        }
        #endregion

        #region Customer
        [Authorize]
        public ActionResult CustomerDetails(string userName)
        {
            var customer = _userService.Get(userName);
            var orders = _orderService.GetOrdersByCustomer(customer.Entity.Id);
            var model = new CustomerDetailsViewModel()
            {
                Orders = orders.Select(o => new OrderModel { Id = o.Id, DeliveryDate = o.DeliveryDate, IsReady = o.IsReady }).ToList(),
                Customer = customer.Entity
            };
            
                return View("~/Views/Admin/CustomerDetails.cshtml", model);
        }
        [HttpPost]
        public ActionResult CustomerDetails(CustomerDetailsViewModel model)
        {
            foreach(var item in model.Orders)
            {
                var order = _orderService.GetOrderById(item.Id);
                order.IsReady = item.IsReady;
                _orderService.UpdateOrder(order);
            }

            return RedirectToAction("CustomerList");
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

            return View("~/Views/User/SelectParts.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SelectParts(CustomizeProductViewModel viewModel)
        {
            var selectedPartList = viewModel.Product.Parts.Where(p => p.IsSelected == true).ToList();

            if(selectedPartList!=null)
            {
                var deliveryDate = DateTime.Now.AddDays(selectedPartList.OrderByDescending(d => d.LeadTime).First().LeadTime);
                
                int productId = viewModel.Product.Id;
                StringBuilder productCode = new StringBuilder();
                decimal productPrice = 0;
                bool isReady = false;
                var userID = _userService.Get(HttpContext.User.Identity.Name).Entity;

                foreach(var part in selectedPartList)
                {
                    productPrice += part.Price;
                    isReady = deliveryDate > DateTime.Now ? false : true;
                    productCode.Append(part.StockKeepingUnit.Substring(0,2));
                }

                var order = new Order {
                    Product = _productService.GetProduct(viewModel.Product.Id),
                    DeliveryDate = deliveryDate, 
                    IsReady = isReady, 
                    Price = productPrice, 
                    ProductId = productId, 
                    User = userID };

                _orderService.AddOrder(order);
                
            }

            return View("~/Views/User/SelectParts.cshtml");
        }

        [HttpGet]
        public JsonResult GetIncompatibleParts(int productId, int partId)
        {
            var product = _productService.GetProduct(productId);

            var ip = product.Parts.SingleOrDefault(p => p.Id == partId).IncompatibleParts.Select(p => p.Id);

            return Json(new { ip = ip }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ConfirmOrder()
        {
            return View("~/Views/User/ConfirmOrder.cshtml");
        }

        [Authorize]
        public ActionResult OrderList()
        {
            var orders = _orderService.GetOrdersByCustomer(CurrentUser.Id);
            var model = new CustomerDetailsViewModel()
            {
                Orders = orders.Select(o => new OrderModel { Id = o.Id, DeliveryDate = o.DeliveryDate, IsReady = o.IsReady }).ToList(),
                Customer = CurrentUser
            };
            
            return View("~/Views/User/OrderList.cshtml", model);
        }
        [Authorize]
        public ActionResult ConfigureProduct()
        {
            var viewModel = new CustomizeProductViewModel { Products = _productService.GetAllProducts().Select(p => new ProductViewModel(p)).ToList() };

            return View("~/Views/User/ConfigureProduct.cshtml", viewModel);
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
                var viewModel = new ProductListViewModel
                {
                    Products = _productService.GetAllProducts()
                };

                return View("~/Views/Admin/AdminProductList.cshtml", viewModel);
            }

            return View("~/Views/User/ProductList.cshtml");
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

            var response = _userService.RegisterUser(viewModel.Username, 
                viewModel.Password, viewModel.ConfirmPassword, userDetails);

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

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            if(filterContext.HttpContext.User != null && filterContext
                .HttpContext.User.Identity.AuthenticationType == AuthenticationMode.Forms.ToString())
            {
                _authenticationService.AuthenticateRequest(filterContext.HttpContext);
            }
        }
    }
}