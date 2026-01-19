using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class FeatureController : Controller
    {

        FeatureManager featureManager = new FeatureManager(new EfFeatureDal());

        private readonly UserManager<WriterUser> _userManager;
        public FeatureController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = featureManager.TGetList().Where(f => f.User == user.UserName).FirstOrDefault();


            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Feature feature)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            feature.User = User.Identity.Name;

            featureManager.TUpdate(feature);
            TempData["SuccessMessage"] = "Başlık kısmı başarıyla keydedildi değişiklik için siteyi ziyaret edebilirsiniz.";
            return RedirectToAction("Index");
        }
    }
}
