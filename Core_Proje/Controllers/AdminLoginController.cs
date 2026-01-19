using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    [AllowAnonymous] // Allow access without authentication
    public class AdminLoginController : Controller
    {
        Context context = new Context();

        // Dashboard view (requires authentication)
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // This controller is disabled (note: comment indicates it might not be in active use)

        // Authentication action for admin login
        public async Task<IActionResult> ToDashBoard(Admin a)
        {
            // Validate admin credentials against database
            var user = context.Admins.Where(u => u.userName == a.userName && u.password == a.password).FirstOrDefault();

            if (user != null)
            {
                // Create claims for authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userName)
                };

                // Create identity and principal for authentication
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user using cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Set session variable for authorization level
                HttpContext.Session.SetString("yetki", "1");

                // Redirect to admin dashboard
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Add error message for invalid credentials
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
            }

            return RedirectToAction("Index");
        }
    }
}