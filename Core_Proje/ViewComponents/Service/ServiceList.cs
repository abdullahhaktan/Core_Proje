using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.Service
{
    public class ServiceList : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public ServiceList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }


        ServiceManager serviceManager = new ServiceManager(new EfServiceDal());
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            var values = serviceManager.TGetList().Where(s => s.User == user.UserName).ToList();
            return View(values);
        }
    }
}
