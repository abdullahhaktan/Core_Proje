using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core_Proje.ViewComponents.Portfolio
{
    public class SlideList : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public SlideList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }
        public string userName()
        {
            var user = User.Identity.Name;

            return user;
        }

        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());
        public IViewComponentResult Invoke()
        {
            var user = userName();

            var values = portfolioManager.TGetList().Where(p => p.User == user).ToList();
            return View(values);
        }
    }
}