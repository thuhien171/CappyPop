using DOTNETMVC.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace DOTNETMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger; // Correct logger to match controller name

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        #region Product List
        public IActionResult ProductList()
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var productListViewModel = new ProductListViewModel(); // Step 1: create the object
                var products = db.Products.ToList(); // Step 2: load data from the 'Products' DbSet
                productListViewModel.Products = products; // Step 3: assign data to the object
                return View(productListViewModel); // Step 4: return the View with ViewModel
            }
        }
        #endregion

        #region Create Product
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveNewProduct(FormSaveNewProduct formData)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(formData.Name)) // Check for valid data
            {
                using (var db = new Dotnet1BlogDemoContext())
                {
                    db.Products.Add(new Product
                    {
                        Name = formData.Name,
                        Price = formData.Price,
                        Description = formData.Description
                    });

                    db.SaveChanges();

                    return RedirectToAction("ProductList", "Product"); // Use RedirectToAction for cleaner routing
                }
            }

            // Return to the create page if model is invalid or name is null
            return View("CreateProduct", formData); // Changed to return the model data back to the view
        }
        #endregion

        #region Edit/Update Product
        public IActionResult UpdateProduct(int id)
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var product = db.Products.FirstOrDefault(e => e.ProductId == id);
                if (product != null)
                {
                    var viewModel = new UpdateProductViewModel
                    {
                        Product = product
                    };

                    return View(viewModel);
                }

                return RedirectToAction("ProductList", "Product"); // Redirect to product list if not found
            }
        }

        [HttpPost]
        public IActionResult SaveUpdateProduct(FormSaveUpdateProduct formData)
        {
            if (ModelState.IsValid && formData.ProductId > 0 && !string.IsNullOrEmpty(formData.Name))
            {
                using (var db = new Dotnet1BlogDemoContext())
                {
                    // Update existing product in the database
                    var product = db.Products.FirstOrDefault(p => p.ProductId == formData.ProductId);
                    if (product != null)
                    {
                        product.Name = formData.Name;
                        product.Price = formData.Price;
                        product.Description = formData.Description;

                        db.SaveChanges();

                        // Redirect to product list view
                        return RedirectToAction("ProductList", "Product");
                    }
                }
            }

            // Redirect to create product view if data is invalid or name is null
            return RedirectToAction("CreateProduct", "Product");
        }
        #endregion

        #region Delete Product
        public IActionResult DeleteProduct(int id)
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var product = db.Products.FirstOrDefault(e => e.ProductId == id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }

                return RedirectToAction("ProductList", "Product"); // Redirect to product list after delete
            }
        }
        #endregion
    }

    #region Product List ViewModel
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
    }
    #endregion

    #region Create Product ViewModel
    public class FormSaveNewProduct
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
    #endregion

    #region Edit/Update Product ViewModel
    public class UpdateProductViewModel
    {
        public Product? Product { get; set; }
    }

    public class FormSaveUpdateProduct
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
    #endregion
}