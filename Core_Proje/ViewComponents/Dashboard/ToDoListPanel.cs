using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class ToDoListPanel : ViewComponent
    {
        public string userName()
        {
            var user = User.Identity.Name;

            return user;
        }

        ToDoListManager toDoListManager = new ToDoListManager(new EfToDoListDal());
        public IViewComponentResult Invoke()
        {
            string userName1 = userName();
            var values = toDoListManager.TGetList().Where(t => t.User == userName1).ToList();
            return View(values);
        }
    }
}
