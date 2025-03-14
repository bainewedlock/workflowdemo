using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WorkerServiceDemo.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{

    [TempData]
    public string ErrorMessage { get; set; }
    public string ReturnUrl { get; set; }
    [BindProperty, Required]
    public string Username { get; set; }
    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; }

    public void OnGet(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        ReturnUrl = returnUrl;
    }

    class UserModel
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Password { get; internal set; }
        public string Role { get; internal set; }
    }

    List<UserModel> users = new()
    {
        new UserModel { Id = 1337, Name = "boeckwi", Password = "abc", Role = "Admin" }
    };

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = users.SingleOrDefault(u => u.Name == Username && u.Password == Password);
            user = users[0];

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties { IsPersistent = true }); // keep cookie when browser is closed

                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    string sha256(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var inputHash = SHA256.HashData(inputBytes);
        return Convert.ToHexString(inputHash);
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}
