using System;
using System.ComponentModel.DataAnnotations;
using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dashboard.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;


        public RegisterModel(ApplicationDbContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

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
          
            
            if (await _userService.IsEmailTakenAsync(Email))
            {
                Message = "This email is already in use.";
                return Page();
            }

            if(Password != ConfirmPassword)
            {
                Message = "This password dose not match";
                return Page();
            }

            try
            {
                var user = await _userService.CreateUserAsync(FullName, Email, Password);
                
                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                Message = "Something went wrong: " + ex.Message;
                return Page();
            }
        }
    }
}
