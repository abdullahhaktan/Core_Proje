using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class StatisticsDashboard2 : ViewComponent
    {
        public string userName()
        {
            var user = User.Identity.Name;

            return user;
        }

        Context c = new Context();
        public IViewComponentResult Invoke()
        {

            var userName1 = userName();

            ViewBag.v1 = c.Portfolios.Where(u => u.User == userName1).Count();
            ViewBag.v2 = c.WriterMessages.Where(m => m.ReceiverName == userName1).Count();
            ViewBag.v3 = c.Services.Where(s => s.User == userName1).Count();
            return View();
        }
    }
}
