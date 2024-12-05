using DOTNETMVC.Models.Tables;
using Microsoft.AspNetCore.Mvc;

namespace DOTNETMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        #region Categories
        public IActionResult Categories()
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var categoriesViewModel = new CategoriesViewModel(); // bước 1 tạo đối tượng
                var categories = db.Categories.ToList(); // bước 2 load dữ liệu từ db
                categoriesViewModel.categories = categories; // bước 3 gắn lại dữ liệu vào đối tượng
                return View(categoriesViewModel); // bước 4 return l
            }
        }
        #endregion

        #region User List
        public IActionResult UserList()
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var userListViewModel = new UserListViewModel(); // Step 1: create the object
                var userList = db.Users.ToList(); // Step 2: load data from the 'Users' DbSet
                userListViewModel.Users = userList; // Step 3: assign data to the object
                return View(userListViewModel); // Step 4: return the View with ViewModel
            }
        }
        #endregion

        #region Create Category
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public RedirectResult SaveNewCategory(FormSaveNewCategory formData)
        {
            if (ModelState.IsValid && formData.Name != null)
            {
                using (var db = new Dotnet1BlogDemoContext())
                {
                    db.Categories.Add(new Category
                    {
                        Name = formData.Name,
                        Description = formData.Description
                    });

                    db.SaveChanges();

                    return new RedirectResult(url: "/admin/categories");
                }
            }

            // Return to the create page if model is invalid or name is null
            return new RedirectResult(url: "/admin/createCategory");
        }
        #endregion

        #region Edit/Update Category
        public IActionResult UpdateCategory(int id)
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var category = db.Categories.FirstOrDefault(e => e.Id == id);
                if (category != null)
                {
                    var viewModel = new UpdateCategoriesViewModel
                    {
                        Category = category
                    };

                    return View(viewModel);
                }

                return new RedirectResult(url: "/admin/categories");
            }
        }

        public RedirectResult SaveUpdateCategory(FormSaveUpdateCategory formData)
        {
            if (formData.Name != null)
            {
                using (var db = new Dotnet1BlogDemoContext())
                {
                    // Update existing category in the database
                    var category = db.Categories.FirstOrDefault(c => c.Id == formData.Id);
                    if (category != null)
                    {
                        category.Name = formData.Name;
                        category.Description = formData.Description;

                        db.SaveChanges();
                        
                        // Redirect to categories view
                        return new RedirectResult(url: "/admin/categories");
                    }
                }
            }

            // Redirect to create category view if the name is null
            return new RedirectResult(url: "/admin/createCategory");
        }
        #endregion
        
        #region Delete Category
        public RedirectResult DeleteCategory(int id)
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var category = db.Categories.FirstOrDefault(e => e.Id == id);
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }

                return new RedirectResult(url: "/admin/categories");
            }
        }
        #endregion

    }

    #region Categories ViewModel
    public partial class CategoriesViewModel 
    {
        public List<Category> categories { get; set; } = new List<Category>();
    }
    #endregion

    #region User List ViewModel
    public class UserListViewModel
    {
        public List<User>? Users { get; set; }= new List<User>();
    }
    #endregion

    #region Create Category ViewModel
    public partial class CreateCategoriesViewModel
        {
            public Category? Category { get; set; }
        }

    public partial class FormSaveNewCategory
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    #endregion

    #region Edit/Update Category ViewModel
    public partial class UpdateCategoriesViewModel
    {
        public Category? Category { get; set; }
    }

    public class FormSaveUpdateCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    #endregion
    
}