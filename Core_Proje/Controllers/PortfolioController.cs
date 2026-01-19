using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;
        public PortfolioController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Helper method to get current username
        public string userName()
        {
            var user = User.Identity.Name;
            return user;
        }

        // Manager for portfolio operations
        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());

        // Display all portfolios for current user
        public async Task<IActionResult> Index()
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;
            ViewBag.imageUrl = user1.ImageUrl;

            var userName = user1.UserName;

            // Get portfolios filtered by current user
            var values = portfolioManager.TGetList().Where(p => p.User == userName).ToList();
            return View(values);
        }

        // Display form for adding new portfolio (GET)
        [HttpGet]
        public IActionResult AddPortfolio()
        {
            return View();
        }

        // Handle form submission for adding portfolio (POST)
        [HttpPost]
        public async Task<IActionResult> AddPortfolio(Portfolio portfolio)
        {
            // Handle main image upload
            if (portfolio.Image != null && portfolio.Image.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image.CopyToAsync(stream);
                }

                portfolio.ImageUrl = "/userimage/" + newImageName;
            }

            // Handle secondary image upload
            if (portfolio.Image1 != null && portfolio.Image1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image1.CopyToAsync(stream);
                }

                portfolio.ImageUrl1 = "/userimage/" + newImageName;
            }

            // Handle first screenshot image upload
            if (portfolio.screenShotImage != null && portfolio.screenShotImage.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl = "/userimage/" + newImageName;
            }

            // Handle second screenshot image upload
            if (portfolio.screenShotImage1 != null && portfolio.screenShotImage1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage1.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl1 = "/userimage/" + newImageName;
            }

            // Handle third screenshot image upload
            if (portfolio.screenShotImage2 != null && portfolio.screenShotImage2.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage2.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage2.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl2 = "/userimage/" + newImageName;
            }

            // Associate portfolio with current user
            portfolio.User = userName();

            // Validate portfolio using FluentValidation
            PortfolioValidator validations = new PortfolioValidator();
            ValidationResult results = validations.Validate(portfolio);

            if (results.IsValid)
            {
                // Add valid portfolio to database
                portfolioManager.TAdd(portfolio);
                return RedirectToAction("Index");
            }
            else
            {
                // Add validation errors to ModelState
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            return RedirectToAction("Index", "Portfolio");
        }

        // Delete portfolio by ID
        public IActionResult DeletePortfolio(int id)
        {
            var values = portfolioManager.TGetByID(id);
            portfolioManager.TDelete(values);
            return RedirectToAction("Index");
        }

        // Display form for editing portfolio (GET)
        public async Task<IActionResult> EditPortfolio(int id)
        {
            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = portfolioManager.TGetByID(id);
            return View(values);
        }

        // Handle form submission for editing portfolio (POST)
        [HttpPost]
        public async Task<IActionResult> EditPortfolio(Portfolio portfolio)
        {
            var userName1 = userName();

            // Get existing image URLs for fallback
            var ImageUrl = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.ImageUrl).FirstOrDefault();
            var ImageUrl1 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.ImageUrl1).FirstOrDefault();
            var screenShortImageUrl = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl).FirstOrDefault();
            var screenShortImageUrl1 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl1).FirstOrDefault();
            var screenShortImageUrl2 = portfolioManager.TGetList().Where(x => x.User == userName1 && x.PortfolioID == portfolio.PortfolioID).Select(x => x.screenShotImageUrl2).FirstOrDefault();

            // Handle main image upload (with fallback to existing)
            if (portfolio.Image != null && portfolio.Image.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image.CopyToAsync(stream);
                }

                portfolio.ImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                portfolio.ImageUrl = ImageUrl; // Keep existing image
            }

            // Handle secondary image upload (with fallback)
            if (portfolio.Image1 != null && portfolio.Image1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.Image1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.Image1.CopyToAsync(stream);
                }

                portfolio.ImageUrl1 = "/userimage/" + newImageName;
            }
            else
            {
                portfolio.ImageUrl1 = ImageUrl1; // Keep existing image
            }

            // Handle first screenshot upload (with fallback)
            if (portfolio.screenShotImage != null && portfolio.screenShotImage.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl = "/userimage/" + newImageName;
            }
            else
            {
                portfolio.screenShotImageUrl = screenShortImageUrl; // Keep existing image
            }

            // Handle second screenshot upload
            if (portfolio.screenShotImage1 != null && portfolio.screenShotImage1.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage1.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage1.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl1 = "/userimage/" + newImageName;
            }

            // Handle third screenshot upload
            if (portfolio.screenShotImage2 != null && portfolio.screenShotImage2.Length > 0)
            {
                var extension = Path.GetExtension(portfolio.screenShotImage2.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userimage/", newImageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await portfolio.screenShotImage2.CopyToAsync(stream);
                }

                portfolio.screenShotImageUrl2 = "/userimage/" + newImageName;
            }

            // Associate portfolio with current user
            portfolio.User = userName();

            // Validate portfolio using FluentValidation
            PortfolioValidator validations = new PortfolioValidator();
            ValidationResult results = validations.Validate(portfolio);

            if (results.IsValid)
            {
                // Update valid portfolio in database
                portfolioManager.TUpdate(portfolio);
                return RedirectToAction("Index");
            }
            else
            {
                // Add validation errors to ModelState
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return RedirectToAction("EditPortfolio", "Portfolio");
        }
    }
}