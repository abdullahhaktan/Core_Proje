using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class SocialMediaController : Controller
    {
        // Manager for social media operations
        SocialMediaManager socialMediaManager = new SocialMediaManager(new EfSocialMediaDal());

        // Helper method to assign Font Awesome icons based on platform name
        public string addIkon(SocialMedia p)
        {
            // Dictionary mapping social media platform names to Font Awesome icon classes
            var ikonlar = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "instagram", "fa fa-instagram" },
                { "facebook", "fa fa-facebook" },
                { "twitter", "fa fa-twitter" },
                { "linkedin", "fa fa-linkedin" },
                { "youtube", "fa fa-youtube" },
                { "github", "fa fa-github" },
                { "tiktok", "fa fa-tiktok" },
                { "pinterest", "fa fa-pinterest" },
                { "snapchat", "fa fa-snapchat" },
                { "telegram", "fa fa-telegram" },
                { "reddit", "fa fa-reddit" },
                { "tumblr", "fa fa-tumblr" },
                { "medium", "fa fa-medium" },
            };

            // Assign appropriate icon if platform name exists in dictionary
            if (!string.IsNullOrEmpty(p.Name) && ikonlar.ContainsKey(p.Name.Trim()))
            {
                p.Icon = ikonlar[p.Name.Trim()];
            }
            return p.Icon;
        }

        private readonly UserManager<WriterUser> _userManager;
        public SocialMediaController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Display all social media links for current user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Set user profile information for display
            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            // Get social media links filtered by current user
            var values = socialMediaManager.TGetList().Where(s => s.User == user.UserName).ToList();
            return View(values);
        }

        // Display form for adding new social media link (GET)
        [HttpGet]
        public async Task<IActionResult> AddSocialMedia()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            return View();
        }

        // Handle form submission for adding social media link (POST)
        [HttpPost]
        public async Task<IActionResult> AddSocialMedia(SocialMedia p)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Assign appropriate icon based on platform name
            p.Icon = addIkon(p);

            // Set status to active
            p.Status = true;

            // Associate with current user
            p.User = user.UserName;

            // Add to database
            socialMediaManager.TAdd(p);
            return RedirectToAction("Index");
        }

        // Delete social media link by ID
        public IActionResult DeleteSocialMedia(int id)
        {
            var values = socialMediaManager.TGetByID(id);
            socialMediaManager.TDelete(values);
            return RedirectToAction("Index");
        }

        // Display form for editing social media link (GET)
        [HttpGet]
        public async Task<IActionResult> EditSocialMedia(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = socialMediaManager.TGetByID(id);
            return View(values);
        }

        // Handle form submission for editing social media link (POST)
        [HttpPost]
        public async Task<IActionResult> EditSocialMedia(SocialMedia p)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Associate with current user
            p.User = user.UserName;

            // Update icon based on platform name
            p.Icon = addIkon(p);

            // Update in database
            socialMediaManager.TUpdate(p);
            return RedirectToAction("Index");
        }
    }
}