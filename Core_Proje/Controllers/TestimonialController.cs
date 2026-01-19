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
    public class TestimonialController : Controller
    {
        // Manager for testimonial operations
        TestimonialManager testimonialManager = new TestimonialManager(new EfTestimonialDal());

        private readonly UserManager<WriterUser> _userManager;
        public TestimonialController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Display all testimonials for current user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Set user profile information for display
            ViewBag.imageUrl = user.ImageUrl;
            ViewBag.nameSurname = user.Name + " " + user.Surname;

            // Get testimonials filtered by current user
            var values = testimonialManager.TGetList().Where(t => t.User == user.UserName).ToList();
            return View(values);
        }

        // Display form for adding new testimonial (GET)
        [HttpGet]
        public async Task<IActionResult> AddTestimonial()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;
            return View();
        }

        // Handle form submission for adding testimonial (POST)
        [HttpPost]
        public async Task<IActionResult> AddTestimonial(Testimonial testimonial)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Handle image upload if provided
            if (testimonial.Image != null && testimonial.Image.Length > 0)
            {
                var extension = Path.GetExtension(testimonial.Image.FileName);
                var newImageName = Guid.NewGuid() + extension; // Generate unique filename
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                // Save uploaded image to server
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await testimonial.Image.CopyToAsync(stream);
                }

                testimonial.ImageUrl = "/userimage/" + newImageName;
            }

            // Associate testimonial with current user
            testimonial.User = user.UserName;

            // Add testimonial to database
            testimonialManager.TAdd(testimonial);

            return RedirectToAction("Index");
        }

        // Delete testimonial by ID
        public IActionResult DeleteTestimonial(int id)
        {
            var values = testimonialManager.TGetByID(id);
            testimonialManager.TDelete(values);
            return RedirectToAction("Index");
        }

        // Display form for editing testimonial (GET)
        [HttpGet]
        public async Task<IActionResult> EditTestimonial(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            // Set user profile information for display
            ViewBag.imageUrl = user.ImageUrl;
            ViewBag.nameSurname = user.Name + " " + user.Surname;

            var values = testimonialManager.TGetByID(id);
            return View(values);
        }

        // Handle form submission for editing testimonial (POST)
        [HttpPost]
        public async Task<IActionResult> EditTestimonial(Testimonial testimonial)
        {
            // Handle image upload if new image provided
            if (testimonial.Image != null && testimonial.Image.Length > 0)
            {
                var extension = Path.GetExtension(testimonial.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await testimonial.Image.CopyToAsync(stream);
                }

                testimonial.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                // Keep existing image if no new image uploaded
                var referencee = testimonialManager.TGetByID(testimonial.TestimonialID);
                testimonial.ImageUrl = referencee.ImageUrl; // Preserve existing image
            }

            // Update testimonial in database
            testimonialManager.TUpdate(testimonial);
            return RedirectToAction("Index");
        }
    }
}