using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    [AllowAnonymous] // Allows access without authentication (note: might be intentional for this area)

    public class DashboardController : Controller
    {
        // User manager for writer user operations
        private readonly UserManager<WriterUser> _userManager;

        // Managers for about and feature sections
        AboutManager about = new AboutManager(new EfAboutDal());
        FeatureManager feature = new FeatureManager(new EfFeatureDal());

        // Constructor with dependency injection for user manager
        public DashboardController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        // Main dashboard action
        public async Task<IActionResult> Index()
        {
            // Get current logged-in user
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.v = values.Name + " " + values.Surname;

            string userName = values.UserName;

            // Weather API integration
            string api = "XXXXXXXXXXXXXXXXXXXXX"; // API key should be stored securely
            string connection = "http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=" + api;
            XDocument document = XDocument.Load(connection);
            ViewBag.v5 = document.Descendants("temperature").ElementAt(0).Attribute("value").Value;

            // Statistics for dashboard
            Context c = new Context();
            // Uncommented: ViewBag.v1 = c.WriterMessages.Where(m => m.Receiver == values.Email).Count();
            ViewBag.v2 = c.Announcements.Count(); // Total announcements count
            ViewBag.v3 = c.Users.Count(); // Total users count
            ViewBag.v4 = c.Skills.Where(s => s.User == userName).Count(); // User's skills count
            return View();
        }

        // Action to check if user has profile setup complete
        public async Task<IActionResult> Vitrin()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            // Get user-specific about and feature data
            var aboutValues = about.TGetList().Where(about => about.User == values.UserName).ToList();
            var featureValues = feature.TGetList().Where(feature => feature.User == values.UserName).ToList();

            // Check if profile is complete (has both about and feature data)
            if ((aboutValues.Count == 0) || (featureValues.Count == 0))
            {
                // Set success message in session
                HttpContext.Session.SetString("SuccessMessage", "İşlem başarılı!");
                return RedirectToAction("Index", new { msg = "İşlem başarılı!" });
            }

            // If profile complete, redirect to default page
            return RedirectToAction("Index", "Default");
        }

        // Action to redirect to admin dashboard
        public async Task<IActionResult> Admin()
        {
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
/*
 Weather API example URL (commented out):
 http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=14ad2aba611dbef9c504b82a127794c5
 */