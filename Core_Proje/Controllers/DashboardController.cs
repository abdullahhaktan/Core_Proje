using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;
        ToDoListManager todoManager = new ToDoListManager(new EfToDoListDal());

        public DashboardController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user.Name + " " + user.Surname;
            ViewBag.imageUrl = user.ImageUrl;

            var values = todoManager.TGetList().Where(u => u.User == user.UserName).ToList();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDoList toDo)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            toDo.Status = true;
            toDo.User = user.UserName;
            todoManager.TAdd(toDo);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSelectedTodos(List<int> selectedIds)
        {
            foreach (var id in selectedIds)
            {
                var todo = todoManager.TGetByID(id);
                if (todo != null)
                {
                    todoManager.TDelete(todo);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
