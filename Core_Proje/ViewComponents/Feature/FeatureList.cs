using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.Feature
{
    public class FeatureList : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public FeatureList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }



        FeatureManager featureManager = new FeatureManager(new EfFeatureDal());
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var values = featureManager.TGetList().Where(u => u.User == user.UserName).ToList();
            return View(values);
        }
    }
}
