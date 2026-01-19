using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class Last5Projects : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;

        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());
        public string userName()
        {
            var user = User.Identity.Name;

            return user;
        }

        public Last5Projects(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userName1 = userName();

            var last5Projects = portfolioManager.TGetList()
                .Where(p => p.User == userName1).OrderByDescending(p => p.PortfolioID)
                .Take(5).ToList();

            return View(last5Projects);
        }
    }
}
