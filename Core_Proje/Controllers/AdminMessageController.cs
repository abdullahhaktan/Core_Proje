using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core_Proje.Controllers
{
    public class AdminMessageController : Controller
    {
        WriterMessageManager writerMessageManager = new WriterMessageManager(new EfWriterMessageDal());


        private readonly UserManager<WriterUser> _userManager;
        public AdminMessageController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<IActionResult> SenderMessageList()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            ViewBag.nameSurname = user.Name + " " + user.Surname;

            string p;
            p = "admin@gmail.com";
            //var values = writerMessageManager.GetListReceiverMessage(p)/*.Where(m=>m.User == user.UserName)*/.ToList();
            return View(/*values*/);
        }
        //public async Task<IActionResult> SenderMessageList()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);

        //    ViewBag.imageUrl = user.ImageUrl;

        //    ViewBag.nameSurname = user.Name + " " + user.Surname;

        //    string p;
        //    p = "admin@gmail.com";
        //    //var values = writerMessageManager.GetListSenderMessages(p)/*.Where(m => m.User == user.UserName)*/.ToList(); ;
        //    return View(/*values*/);
        //}
        public IActionResult AdminMessageDetails(int id)
        {
            //var values = writerMessageManager.TGetByID(id);
            return View(/*values*/);
        }
        public IActionResult AdminMessageDelete(int id)
        {
            //var values = writerMessageManager.TGetByID(id);
            //writerMessageManager.TDelete(values);
            return RedirectToAction("SenderMessageList");
        }
        [HttpGet]
        public async Task<IActionResult> AdminMessageSend()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.imageUrl = user.ImageUrl;

            ViewBag.nameSurname = user.Name + " " + user.Surname;

            return View();
        }
        //[HttpPost]
        //public IActionResult AdminMessageSend(WriterMessage p)
        //{
        //    p.Sender = "admin@gmail.com";
        //    p.SenderName = "Admin";
        //    p.Date = DateTime.Parse(DateTime.Now.ToShortDateString());
        //    Context c = new Context();
        //    var usernamesurname = c.Users.Where(u => u.Email == p.Receiver).Select(u => u.Name + " " + u.Surname).FirstOrDefault();

        //    p.ReceiverName = usernamesurname;
        //    writerMessageManager.TAdd(p);
        //    return RedirectToAction("SenderMessageList");
        //}
    }
}
