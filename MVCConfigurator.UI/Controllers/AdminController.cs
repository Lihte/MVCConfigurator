using MVCConfigurator.Domain.Models;
using MVCConfigurator.Domain.Services;
using MVCConfigurator.UI.Models;
using MVCConfigurator.UI.Security;
using MVCConfigurator.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCConfigurator.UI.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public User CurrentUser { get { return System.Web.HttpContext.Current.User as User; } }

        public AdminController(IUserService userService,
            IProductService productService, IOrderService orderService, IAuthenticationService authenticationService)
            : base(authenticationService)
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
        
        [CustomAuthAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthAttribute]
        public ActionResult CreateProduct()
        {
            var viewModel = new ProductViewModel();

            return View(viewModel);
        }

        [CustomAuthAttribute]
        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else if (_productService.GetAllProductCategories().Any(c => c.Name == model.Product.Category))
            {
                ModelState.AddModelError("Product.Category", "Category already exists");
                return View();
            }

            var product = new Product()
            {
                Name = model.Product.Category,
                Category = new ProductCategory { Name = model.Product.Category },
                ImagePath = SaveImage(model.Product.Image.ImageUpload)
            };

            product = _productService.AddProduct(product);

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }

        [CustomAuthAttribute]
        public ActionResult ProductDetails()
        {
            return View();
        }

        [CustomAuthAttribute]
        public ActionResult ProductList()
        {
            var viewModel = new ProductListViewModel
            {
                Products = _productService.GetAllProducts()
            };

            return View(viewModel);
        }

        [CustomAuthAttribute]
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

        [CustomAuthAttribute]
        [HttpPost]
        public ActionResult AddPart(PartViewModel model)
        {
            var product = _productService.GetProduct(model.ProductId);

            // Should not be validated if category is not selected from the dropdownlist
            if (model.PartDetails.CategoryId == 0)
            {
                ModelState.Remove("PartDetails.CategoryId");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = new PartViewModel()
                {
                    Categories = _productService.GetAllPartCategoriesByProduct(product)
                    .Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name }),
                    ExistingParts = product.Parts.Select(p => new PartModel(p)).ToList(),
                    ProductId = product.Id
                };

                return View(model);
            }

            // Create a list of incompatible parts based on choices made in view
            var incompatibleParts = new List<Part>();
            if (model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach (var item in model.ExistingParts)
                {
                    if (item.IsIncompatible)
                    {
                        incompatibleParts.Add(product.Parts.First(p => p.Id == item.Id));
                    }
                }
            }

            // All parts of the same category should be considered to be incompatible
            var sameCategory = product.Parts.Where(p => p.Category.Id == model.PartDetails.CategoryId).Select(p => p);
            incompatibleParts.AddRange(sameCategory);

            // Get Part Category if already exists, else create new when building Part model
            var partCategory = product.Parts.FirstOrDefault(p => p.Category.Id == model.PartDetails.CategoryId);

            var part = new Part()
            {
                Category = partCategory != null ? partCategory.Category : new PartCategory { Name = model.PartDetails.Category },
                ImagePath = SaveImage(model.PartDetails.Image.ImageUpload),
                LeadTime = model.PartDetails.LeadTime,
                Name = model.PartDetails.Name,
                Price = model.PartDetails.Price,
                StockKeepingUnit = model.PartDetails.StockKeepingUnit,
                IncompatibleParts = incompatibleParts
            };

            product.Parts.Add(part);
            _productService.UpdateProduct(product);

            part = _productService.GetProduct(model.ProductId).Parts.LastOrDefault();

            // Must go through and update each parts list of incompatible parts
            if (model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach (var item in model.ExistingParts)
                {
                    if (item.IsIncompatible)
                    {
                        var partUpdate = product.Parts.SingleOrDefault(p => p.Id == item.Id);
                        partUpdate.IncompatibleParts.Add(part);
                    }
                }
            }

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }

        [CustomAuthAttribute]
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

        [CustomAuthAttribute]
        [HttpPost]
        public ActionResult EditPart(PartViewModel model)
        {
            var product = _productService.GetProduct(model.ProductId);

            if (model.PartDetails.CategoryId == 0)
            {
                ModelState.Remove("PartDetails.CategoryId");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = new PartViewModel()
                {
                    Categories = _productService.GetAllPartCategoriesByProduct(product)
                    .Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name }),
                    ExistingParts = product.Parts.Select(p => new PartModel(p)).ToList(),
                    ProductId = product.Id
                };

                return View(model);
            }

            // Create a list of incompatible parts based on choices made in view
            var incompatibleParts = new List<Part>();

            if (model.ExistingParts != null && model.ExistingParts.Count > 0)
            {
                foreach (var item in model.ExistingParts)
                {
                    if (item.IsIncompatible)
                    {
                        incompatibleParts.Add(product.Parts.First(p => p.Id == item.Id));
                    }
                }
            }

            // Clear and Update list of incompatible parts and add same category parts to list
            var part = product.Parts.SingleOrDefault(m => m.Id == model.PartDetails.Id);
            _productService.UpdateProduct(product, part);
            var sameCategory = product.Parts.Where(p => (p.Category.Id == part.Category.Id) && (p.Id != part.Id)).Select(p => p);
            incompatibleParts.AddRange(sameCategory);

            part.ImagePath = SaveImage(model.PartDetails.Image.ImageUpload);
            part.LeadTime = model.PartDetails.LeadTime;
            part.Name = model.PartDetails.Name;
            part.Price = model.PartDetails.Price;
            part.StockKeepingUnit = model.PartDetails.StockKeepingUnit;
            part.IncompatibleParts = incompatibleParts;

            // Go through each incompatible part and add this part to its list
            foreach (var p in part.IncompatibleParts)
            {
                p.IncompatibleParts.Add(part);
            }

            _productService.UpdateProduct(product);

            return RedirectToAction("ProductPartList", new { id = product.Id });
        }

        [CustomAuthAttribute]
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
                },
            };

            return View(viewModel);
        }

        [CustomAuthAttribute]
        public ActionResult CustomerList()
        {
            var model = new CustomerListViewModel()
            {
                Users = _userService.GetAllUsers().Where(u => u.IsAdmin == false).ToList()
            };

            return View(model);
        }

        [CustomAuthAttribute]
        public ActionResult CustomerDetails(string userName)
        {
            var customer = _userService.Get(userName);
            var orders = _orderService.GetOrdersByCustomer(customer.Entity.Id);
            var model = new CustomerDetailsViewModel()
            {
                Orders = orders.Select(o => new OrderModel { Id = o.Id, DeliveryDate = o.DeliveryDate, IsReady = o.IsReady }).ToList(),
                Customer = customer.Entity
            };

            return View(model);
        }

        [CustomAuthAttribute]
        [HttpPost]
        public ActionResult CustomerDetails(CustomerDetailsViewModel model)
        {
            if (model.Orders != null)
            {
                foreach (var item in model.Orders)
                {
                    var order = _orderService.GetOrder(item.Id);
                    order.IsReady = item.IsReady;
                    _orderService.UpdateOrder(order);
                }
            }

            return RedirectToAction("CustomerList");
        }

        public string SaveImage(HttpPostedFileBase image)
        {
            if (image == null || String.IsNullOrEmpty(image.FileName))
            {
                return @"~/Content/Images/noimage.jpg";
            }

            var fileName = Path.GetFileName(image.FileName);
            var absolutePath = Url.Content(Path.Combine(Server.MapPath("~/Content/Images"), fileName));

            image.SaveAs(absolutePath);

            return @"~/Content/Images/" + fileName;
        }
    }
}