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
    public class ServiceController : Controller
    {
        // Manager for service operations
        ServiceManager serviceManager = new ServiceManager(new EfServiceDal());

        private readonly UserManager<WriterUser> _userManager;
        public ServiceController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Display all services for current user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Set user profile image for display
            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            // Get services filtered by current user
            var values = serviceManager.TGetList().Where(s => s.User == user.UserName).ToList();
            return View(values);
        }

        // Display form for adding new service (GET)
        [HttpGet]
        public async Task<IActionResult> AddService()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.imageUrl = user.ImageUrl;

            return View();
        }

        // Handle form submission for adding service (POST)
        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Handle image upload if provided
            if (service.Image != null && service.Image.Length > 0)
            {
                var extension = Path.GetExtension(service.Image.FileName);
                var newImageName = Guid.NewGuid() + extension; // Generate unique filename
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                // Save uploaded image to server
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await service.Image.CopyToAsync(stream);
                }

                service.ImageUrl = "/userimage/" + newImageName;
            }

            // Associate service with current user
            service.User = user.UserName;

            // Add service to database
            serviceManager.TAdd(service);

            // Set success message
            TempData["SuccessMessage"] = "Hizmet basariyla eklendi";

            return RedirectToAction("Index");
        }

        // Delete service by ID
        public IActionResult DeleteService(int id)
        {
            var values = serviceManager.TGetByID(id);
            serviceManager.TDelete(values);
            return RedirectToAction("Index");
        }

        // Display form for editing service (GET)
        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.imageUrl = user.ImageUrl;

            var values = serviceManager.TGetByID(id);
            return View(values);
        }

        // Handle form submission for editing service (POST)
        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Handle image upload if new image provided
            if (service.Image != null && service.Image.Length > 0)
            {
                var extension = Path.GetExtension(service.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await service.Image.CopyToAsync(stream);
                }

                service.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                // Fallback to user profile image if no new image uploaded
                // Note: This might not be the intended behavior - should use existing service image
                service.ImageUrl = user.ImageUrl;
            }

            // Associate service with current user
            service.User = user.UserName;

            // Update service in database
            serviceManager.TUpdate(service);

            // Set success message
            TempData["SuccessMessage"] = "Hizmet basariyla guncellendi!";

            return RedirectToAction("Index");
        }
    }
}