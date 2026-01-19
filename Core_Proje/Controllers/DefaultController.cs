using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Core_Proje.Controllers
{
    //[AllowAnonymous]
    public class DefaultController : Controller
    {
        private readonly UserManager<WriterUser> _userManager;
        private readonly IConfiguration _configuration;

        public DefaultController(UserManager<WriterUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private readonly AboutManager _aboutManager = new AboutManager(new EfAboutDal());
        private readonly SocialMediaManager _socialMediaManager = new SocialMediaManager(new EfSocialMediaDal());
        private readonly FeatureManager _featureManager = new FeatureManager(new EfFeatureDal());


        [AllowAnonymous]
        public async Task<IActionResult> AbdullahHaktanCV()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var about = _aboutManager.TGetList().Where(a => a.User == user.UserName).FirstOrDefault();
            var socialMedias = _socialMediaManager.TGetList().Where(s => s.User == user.UserName).FirstOrDefault();

            var aboutValues = _aboutManager.TGetList().Where(about => about.User == user.UserName).ToList();
            var featureValues = _featureManager.TGetList().Where(feature => feature.User == user.UserName).ToList();

            if ((aboutValues.Count == 0) || (featureValues.Count == 0))
            {
                return RedirectToAction("Index", "Dashboard", new { msg = "Vitrin paneline geçmeden önce hakkımda ve başlık kısımlarını eksiksiz tamamlamanız gerekmektedir" });
            }

            ViewBag.job = about.Title;
            ViewBag.Summary = TempData["Summary"];
            ViewBag.nameSurname = user.Name + " " + user.Surname;
            return View();
        }


        public PartialViewResult HeaderPartial()
        {
            return PartialView();
        }

        public PartialViewResult NavbarPartial()
        {
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SendMail(string Name, string Email, string Subject, string MessageBody)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(Name, Email)); // Gönderici bilgileri
                email.To.Add(new MailboxAddress("Abdullah", "abdullahhaktan9@gmail.com")); // Alıcı
                email.Subject = Subject;

                email.Body = new TextPart("plain")
                {
                    Text = $"Gönderen: {Name} ({Email})\n\nMesaj:\n{MessageBody}"
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("ornek@gmail.com.com", "PasswordXXXXX"); // Gmail uygulama şifresi
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return RedirectToAction("AbdullahHaktanCV");
            }
            catch (Exception ex)
            {
                return BadRequest("Mail gönderilirken hata oluştu: " + ex.Message);
            }
        }

    }
}
