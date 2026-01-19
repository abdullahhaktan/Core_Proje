using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class ProjectList : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public ProjectList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        PortfolioManager portfolioManager = new PortfolioManager(new EfPortfolioDal());
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = portfolioManager.TGetList().Where(p => p.User == user.UserName).ToList();
            return View(values);
        }
    }
}
