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
    public class AboutController : Controller
    {
        // Manager for about section operations
        AboutManager aboutManager = new AboutManager(new EfAboutDal());

        private readonly UserManager<WriterUser> _userManager;
        public AboutController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // GET action to display about information for editing
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get current user and their profile image
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.imageUrl = user.ImageUrl;

            // Get user's name and surname for display
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            // Get about data for current user
            var values = aboutManager.TGetList().Where(a => a.User == user.UserName).FirstOrDefault();

            return View(values);
        }

        // POST action to update about information
        [HttpPost]
        public async Task<IActionResult> Index(About about)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Handle image upload if a new image is provided
            if (about.Image != null && about.Image.Length > 0)
            {
                var extension = Path.GetExtension(about.Image.FileName);
                var newImageName = Guid.NewGuid() + extension; // Generate unique filename
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                // Save uploaded image to server
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await about.Image.CopyToAsync(stream);
                }

                // Update about section image URL
                about.ImageUrl = "/userimage/" + newImageName;
            }

            // Set user association and update in database
            about.User = user.UserName;
            aboutManager.TUpdate(about);

            // Set success message in TempData
            TempData["SuccessMessage"] = "Hakkında kısmı basariyla kaydedildi , kontrol etmek için siteyi ziyaret edebilirsiniz.";

            return RedirectToAction("Index");
        }
    }
}