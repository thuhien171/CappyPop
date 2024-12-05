using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DOTNETMVC.Models.Tables;
using Microsoft.EntityFrameworkCore;  // For DbContext methods

namespace DOTNETMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly Dotnet1BlogDemoContext _context;

        // Constructor to inject Dotnet1BlogDemoContext
        public UserController(Dotnet1BlogDemoContext context)
        {
            _context = context;
        }

        #region Register
        // Display registration view
        public IActionResult Register()
        {
            return View();
        }

        // Handle registration POST request
        [HttpPost]
        public IActionResult SaveNewUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new Dotnet1BlogDemoContext())
                {
                    // Hash the password for security
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    // Create and populate the User entity
                    var user = new User
                    {
                        Username = model.Username ?? string.Empty,
                        Email = model.Email ?? string.Empty,      
                        Password = hashedPassword,
                        RememberMe = model.RememberMe,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber,
                        CreatedAt = DateTime.Now
                    };

                    // Save user to the database
                    db.Users.Add(user);
                    db.SaveChanges();

                    // Redirect to the login page after successful registration
                    return new RedirectResult(url: "/user/login");
                }
            }

            // If model validation fails, return to the registration view
            return View("Register", model);
        }
        #endregion

        #region Login
        // Display login view
        public IActionResult Login()
        {
            // Initialize and pass a model to avoid null references
            return View(new LoginViewModel());
        }

        // POST: Handle login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to find the user by email
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) // Secure password check
                {
                    // Set user session (or token-based authentication)
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.Username);

                    // Redirect to profile or dashboard after successful login
                    return RedirectToAction("Profile", new { id = user.Id });
                }
                else
                {
                    // Add model error for invalid credentials
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }

            // Return to login view with the model for any errors
            return View(model);
        }
        #endregion

        #region Profile
        public IActionResult Profile(int id)
        {
            // Retrieve the user from the database using the provided ID
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(); // Handle case where the user doesn't exist
            }

            // Pass user data to the view
            return View(user);
        }
        #endregion

        #region Edit User
        // GET: Edit user details
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                RememberMe = user.RememberMe ?? false
            };

            return View(model);
        }

        // POST: Save user edits
        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Fetch user from the database
                var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
                if (user == null)
                    return NotFound();

                // Update user properties
                user.Username = model.Username ?? string.Empty;
                user.Email = model.Email ?? string.Empty;
                user.PhoneNumber = model.PhoneNumber;
                user.RememberMe = model.RememberMe;
                user.UpdatedAt = DateTime.Now; // Update timestamp

                // Save changes to the database
                _context.Users.Update(user);
                _context.SaveChanges();

                // Redirect to profile page
                return RedirectToAction("Profile", new { id = model.Id });
            }

            return View(model);
        }
        #endregion

        #region Delete User
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            // Fetch the user from the database
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Remove the user and save changes
            _context.Users.Remove(user);
            _context.SaveChanges();

            // Redirect to home or login after deletion
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }

    #region Register ViewModel
    public partial class RegisterViewModel
    {
        // Username can remain optional or required, based on your needs
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }

        public bool RememberMe { get; set; } = false;
    }
    #endregion

    #region Login ViewModel
    public partial class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
    #endregion

    #region Edit User ViewModel
    public partial class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        public bool RememberMe { get; set; }

        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string? FullName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }
    }
    #endregion

}