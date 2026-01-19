using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class FeatureStatistics : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public FeatureStatistics(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }
        Context c = new Context();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.v1 = c.Skills.Where(s => s.User == user.UserName).Count();
            ViewBag.v2 = c.WriterMessages.Where(m => m.ReceiverName == user.UserName).Count();
            ViewBag.v4 = c.Experiences.Count();
            return View();
        }
    }
}
