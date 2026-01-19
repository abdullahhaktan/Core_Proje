using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class ContactController : Controller
    {
        WriterMessageManager messageManager = new WriterMessageManager(new EfWriterMessageDal());

        private readonly UserManager<WriterUser> _userManager;
        public ContactController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            var user1 = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.nameSurname = user1.Name + " " + user1.Surname;

            var values = messageManager.GetListInbox(user1.Email);
            return View(values);
        }
        public IActionResult DeleteContact(int id)
        {
            var values = messageManager.GetByID(id);
            messageManager.MessageDelete(values);
            return RedirectToAction("Index");
        }

        public IActionResult ContactDetails(int id)
        {
            var values = messageManager.GetByID(id);
            return View(values);
        }
    }
}
