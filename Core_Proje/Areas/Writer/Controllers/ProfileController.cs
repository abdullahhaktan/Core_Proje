using Core_Proje.Areas.Writer.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Route("Writer/[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;

        public ProfileController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // GET action to display profile edit form
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get current logged-in user
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            // Populate view model with user data
            UserEditViewModel model = new UserEditViewModel();
            model.Name = values.Name;
            model.Surname = values.Surname;
            model.PictureURL = values.ImageUrl;
            return View(model);
        }

        // POST action to update profile
        [HttpPost]
        public async Task<IActionResult> Index(UserEditViewModel p)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Update basic user information
            user.Name = p.Name;
            user.Surname = p.Surname;

            // Handle profile picture upload if provided
            if (p.Picture != null && p.Picture.Length > 0)
            {
                var extension = Path.GetExtension(p.Picture.FileName);
                var newImageName = Guid.NewGuid() + extension; // Generate unique filename
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                // Save uploaded image to server
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await p.Picture.CopyToAsync(stream);
                }

                // Update user's image URL
                user.ImageUrl = "/userimage/" + newImageName;
            }

            // Handle password update (only if new password provided)
            if (string.IsNullOrEmpty(p.Password))
            {
                // Keep existing password if no new password provided
                p.Password = user.PasswordHash;
            }
            else
            {
                // Hash and set new password
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, p.Password);
            }

            // Update user in database
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Redirect to login page after successful update
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}