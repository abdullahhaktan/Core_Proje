using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.About
{
    public class AboutList : ViewComponent
    {
        ServiceManager serviceManager = new ServiceManager(new EfServiceDal());
        FeatureManager featureManager = new FeatureManager(new EfFeatureDal());
        ExperienceManager experienceManager = new ExperienceManager(new EfExperienceDal());
        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());
        SkillManager skillManager = new SkillManager(new EfSkillDal());
        AboutManager aboutManager = new AboutManager(new EfAboutDal());
        TestimonialManager testimonialManager = new TestimonialManager(new EfTestimonialDal());

        private readonly UserManager<WriterUser> _userManager;
        public AboutList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.ServiceList = serviceManager.TGetList().Where(s => s.User == user.UserName).ToList();
            ViewBag.FeatureList = featureManager.TGetList().Where(f => f.User == user.UserName).ToList();
            ViewBag.ExperienceList = experienceManager.TGetList().Where(e => e.User == user.UserName).ToList();
            ViewBag.PortfolioList = portfolioManager.TGetList().Where(p => p.User == user.UserName).ToList();
            ViewBag.SkillList = skillManager.TGetList().Where(s => s.User == user.UserName).ToList();
            ViewBag.TestimonialList = testimonialManager.TGetList().Where(t => t.User == user.UserName).ToList();


            ViewBag.imageUrl = user.ImageUrl;

            var values = aboutManager.TGetList().Where(u => u.User == user.UserName).ToList();
            return View(values);
        }
    }
}
