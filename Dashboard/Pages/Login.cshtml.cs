using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using Dashboard.Services;

namespace Dashboard.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;

        public LoginModel(ApplicationDbContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public string Message { get; set; }

        public IActionResult OnGet()
        {
            
            var uid = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(uid))
            {
                return RedirectToPage("/Dashboard");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = await _userService.ValidateUserAsync(Email, Password);

            if (user == null)
            {
                Message = "Invalid email or password.";
                return Page();
            }

            
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserFullName", user.FullName);

          

            return RedirectToPage("/Dashboard");
        }
    }
}