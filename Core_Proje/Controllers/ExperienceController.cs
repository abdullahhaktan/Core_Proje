using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    //[Authorize(Roles = "Admin")] // Commented out - currently allows all authenticated users
    public class ExperienceController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;
        public ExperienceController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Manager for experience operations
        ExperienceManager experienceManager = new ExperienceManager(new EfExperienceDal());

        // Display all experiences for current user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Set user profile image for display
            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            // Get experiences filtered by current user
            var values = experienceManager.TGetList().Where(e => e.User == user.UserName).ToList();
            return View(values);
        }

        // Display form for adding new experience (GET)
        [HttpGet]
        public async Task<IActionResult> AddExperience()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            return View();
        }

        // Handle form submission for adding experience (POST)
        [HttpPost]
        public async Task<IActionResult> AddExperience(Experience experience)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Handle image upload if provided
            if (experience.Image != null && experience.Image.Length > 0)
            {
                var extension = Path.GetExtension(experience.Image.FileName);
                var newImageName = Guid.NewGuid() + extension; // Generate unique filename
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                // Save uploaded image to server
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await experience.Image.CopyToAsync(stream);
                }

                experience.ImageUrl = "/userimage/" + newImageName;
            }

            // Associate experience with current user
            experience.User = user.UserName;

            // Add experience to database
            experienceManager.TAdd(experience);

            // Set success message
            TempData["SuccessMessage"] = "Deneyim basariyla eklendi!";

            return RedirectToAction("Index");
        }

        // Delete experience by ID
        public IActionResult DeleteExperience(int id)
        {
            var values = experienceManager.TGetByID(id);
            experienceManager.TDelete(values);
            return RedirectToAction("Index");
        }

        // Display form for editing experience (GET)
        [HttpGet]
        public async Task<IActionResult> EditExperience(int id)
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = experienceManager.TGetByID(id);
            return View(values);
        }

        // Handle form submission for editing experience (POST)
        [HttpPost]
        public async Task<IActionResult> EditExperience(Experience experience)
        {
            // Handle image upload if new image provided
            if (experience.Image != null && experience.Image.Length > 0)
            {
                var extension = Path.GetExtension(experience.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await experience.Image.CopyToAsync(stream);
                }

                experience.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                // Keep existing image if no new image uploaded
                var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
                string imageUrl = user1.ImageUrl; // Note: This uses user profile image, not experience image
                experience.ImageUrl = imageUrl;
            }

            // Set success message
            TempData["SuccessMessage"] = "Deneyim basariyla guncellendi!";

            // Update experience in database
            experienceManager.TUpdate(experience);
            return RedirectToAction("Index");
        }
    }
}