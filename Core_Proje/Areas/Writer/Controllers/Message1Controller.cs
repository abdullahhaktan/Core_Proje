using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Route("Writer/Message1")]

    public class Message1Controller : Controller
    {
        private readonly UserManager<WriterUser> _userManager;

        private readonly WriterMessageManager writerMessageManager = new WriterMessageManager(new EfWriterMessageDal());

        public Message1Controller(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        private async Task SetMessageCountsAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var mail = user?.Email ?? string.Empty;

            var inboxMessages = writerMessageManager.GetListInbox(mail);
            var sendboxMessages = writerMessageManager.GetListSendbox(mail);
            var draftMessages = writerMessageManager.GetListInDraft(mail);
            var trashMessages = writerMessageManager.GetListInTrash(mail);

            var inComingMessageCount = inboxMessages.Count;
            var outComingMessageCount = sendboxMessages.Count;
            var draftMessageCount = draftMessages.Count;
            var trashMessageCount = trashMessages.Count;

            // Session'a yaz
            HttpContext.Session.SetString("inComingMessageCount", inComingMessageCount.ToString());
            HttpContext.Session.SetString("outComingMessageCount", outComingMessageCount.ToString());
            HttpContext.Session.SetString("draftMessageCount", draftMessageCount.ToString());
            HttpContext.Session.SetString("trashMessageCount", trashMessageCount.ToString());

            // ViewBag'e yaz
            ViewBag.inComingMessageCount = inComingMessageCount;
            ViewBag.outComingMessageCount = outComingMessageCount;
            ViewBag.draftMessageCount = draftMessageCount;
            ViewBag.trashMessageCount = trashMessageCount;

        }

        [Route("")]
        [Route("Inbox")]
        public async Task<IActionResult> Inbox(int page = 1)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            var mail = values.Email;

            var allMessages = writerMessageManager.GetListInbox(mail);
            int pageSize = 10;

            var pagedMessages = allMessages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)allMessages.Count / pageSize);
            ViewBag.CurrentPage = page;

            TempData["isSendBox"] = "";

            await SetMessageCountsAsync();

            return View(pagedMessages);
        }

        [Route("")]
        [Route("SendBox")]
        public async Task<IActionResult> SendBox(int page = 1)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            var mail = values.Email;

            var allMessages = writerMessageManager.GetListSendbox(mail);
            int pageSize = 10;

            var pagedMessages = allMessages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)allMessages.Count / pageSize);
            ViewBag.CurrentPage = page;

            TempData["isSendBox"] = true;

            await SetMessageCountsAsync();

            return View(pagedMessages);
        }

        [Route("")]
        [Route("NewMessage")]
        [HttpGet]
        public async Task<ActionResult> NewMessage(string mail = "")
        {
            if (!string.IsNullOrEmpty(mail))
            {
                TempData["SenderMail"] = mail;
            }

            await SetMessageCountsAsync();
            return View();
        }

        WriterUserManager wu = new WriterUserManager(new EfWriterUserDal());

        [Route("")]
        [Route("NewMessageSend")]
        [HttpPost]
        public async Task<ActionResult> NewMessageSend(WriterMessage message)
        {
            var mail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
            message.MessageStatu = 2;
            message.SenderMail = mail;
            message.SenderName = (await _userManager.FindByNameAsync(User.Identity.Name)).Name + " " + (await _userManager.FindByNameAsync(User.Identity.Name)).Surname;

            WriterUser wv = new WriterUser();

            var receiver = wu.GetByEmail(message.ReceiverMail);

            message.ReceiverName = receiver.Name + " " + receiver.Surname;

            message.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            writerMessageManager.MessageAdd(message);
            return RedirectToAction("Inbox");
        }

        [Route("")]
        [Route("Draft")]
        public async Task<ActionResult> Draft(int page = 1)
        {
            var mail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
            var allMessages = writerMessageManager.GetListInDraft(mail);
            int pageSize = 10;

            var pagedMessages = allMessages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)allMessages.Count / pageSize);
            ViewBag.CurrentPage = page;


            await SetMessageCountsAsync();

            return View(pagedMessages);
        }

        [Route("")]
        [Route("GetMessageDetails")]
        [HttpGet]
        public async Task<ActionResult> GetMessageDetails(int id)
        {
            ViewBag.isSendBox = TempData["isSendBox"];
            await SetMessageCountsAsync();
            var messageDetails = writerMessageManager.GetByID(id);
            return View(messageDetails);
        }

        public ActionResult DraftMessage(int id)
        {
            var newMessage = writerMessageManager.GetByID(id);
            return View("NewMessage");
        }

        [Route("")]
        [Route("SaveInDraft")]
        public async Task<ActionResult> SaveInDraft(WriterMessage message)
        {
            await SetMessageCountsAsync();

            message.MessageStatu = 1;
            message.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            writerMessageManager.MessageAdd(message);
            return RedirectToAction("Inbox");
        }

        [Route("")]
        [Route("Trash")]
        public async Task<ActionResult> Trash(int page = 1)
        {
            await SetMessageCountsAsync();

            var mail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
            var allMessages = writerMessageManager.GetListInTrash(mail);
            int pageSize = 10;

            var pagedMessages = allMessages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)allMessages.Count / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedMessages);
        }

        [Route("")]
        [Route("EmptyTrash")]
        public async Task<ActionResult> EmptyTrash()
        {
            var mail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;

            var messages = writerMessageManager.GetListInTrash(mail);
            foreach (var message in messages)
            {
                writerMessageManager.MessageAdd(message);
            }

            return RedirectToAction("Inbox");
        }

        [Route("")]
        [Route("MessageListMenu")]
        public async Task<PartialViewResult> MessageListMenu()
        {
            await SetMessageCountsAsync();

            return PartialView();

        }


        public async Task<IActionResult> ReMessage(string SenderMail)
        {
            await SetMessageCountsAsync();
            var mail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
            TempData["SenderMail"] = SenderMail;
            return RedirectToAction("NewMessage", "Message1", new { area = "Writer" });
        }

    }
}
