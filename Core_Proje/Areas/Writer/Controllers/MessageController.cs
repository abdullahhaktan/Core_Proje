using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core_Proje.Areas.Writer.Controllers
{
    [Area("Writer")]
    [Route("Writer/Message")]

    public class MessageController : Controller
    {
        WriterMessageManager writerMessageManager = new WriterMessageManager(new EfWriterMessageDal());

        private readonly UserManager<WriterUser> _userManager;

        public MessageController(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("")]
        [Route("ReceiverMessage")]
        public async Task<IActionResult> ReceiverMessage(string mail)
        {
            var userValues = await _userManager.FindByNameAsync(User.Identity.Name);
            mail = userValues.Email;
            var messageList = writerMessageManager.GetListInbox(mail);
            return View(messageList);
        }

        [Route("")]
        [Route("SenderMessage")]
        public async Task<IActionResult> SenderMessage(string mail)
        {
            var userValues = await _userManager.FindByNameAsync(User.Identity.Name);
            mail = userValues.Email;
            //var messageList = writerMessageManager.GetListSenderMessages(mail);
            return View(/*messageList*/);
        }

        [Route("MessageDetails/{id}")]
        public IActionResult MessageDetails(int id)
        {
            //WriterMessage writerMessage = writerMessageManager.TGetByID(id);
            return View(/*writerMessage*/);
        }

        [Route("ReceiverMessageDetails/{id}")]
        public IActionResult ReceiverMessageDetails(int id)
        {
            //WriterMessage writerMessage = writerMessageManager.TGetByID(id);
            return View(/*writerMessage*/);
        }

        [HttpGet]
        [Route("")]
        [Route("SendMessage")]
        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(WriterMessage p)
        {
            //var values = await _userManager.FindByNameAsync(User.Identity.Name);
            //string senderMail = values.Email;
            //string senderNameSurname = values.Name + " " + values.Surname;

            //p.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            //p.Sender = senderMail;
            //p.SenderName = senderNameSurname;

            //Context c = new Context();
            //var receiverNameSurname = c.Users.Where(item => item.Email == p.Receiver).Select(y => y.Name + " " + y.Surname).FirstOrDefault();

            //if (receiverNameSurname == null)
            //{
            //    ModelState.AddModelError("Receiver", "Alıcı Maili Bulunamadı.");
            //    return View(p);
            //}

            //p.ReceiverName = receiverNameSurname;

            //// Veritabanına mesajı kaydet
            //writerMessageManager.TAdd(p);

            //// E-posta gönderme işlemi
            //bool emailSent = await SendEmailAsync(p.Receiver, p.Subject, p.MessageContent, senderNameSurname);

            //if (emailSent)
            //{
            //    // E-posta başarıyla gönderildi
            //    TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi ve e-posta ile de iletildi.";
            //}
            //else
            //{
            //    // E-posta gönderilirken bir hata oluştu
            //    //TempData["ErrorMessage"] = "Mesajınız veritabanına kaydedildi ancak e-posta gönderilirken bir sorun oluştu.";
            //}

            return RedirectToAction("SenderMessage");
        }

        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body, string senderNameSurname)
        {
            try
            {
                // SMTP ayarları: Bunları kendi mail sunucunuzun ayarlarına göre düzenlemelisiniz.
                // Örneğin, Gmail için:
                string smtpHost = "smtp.gmail.com"; // Veya kendi SMTP sunucunuz
                int smtpPort = 587; // Genellikle 587 (TLS/STARTTLS) veya 465 (SSL)
                string smtpUsername = "XXXXXXXXXXXXXXXXXXXX"; // Gönderen e-posta adresi
                string smtpPassword = "YYYYYYYYYYYYYYYYYYYY"; // Gönderen e-posta şifresi (uygulama şifresi önerilir)

                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.EnableSsl = true; // SSL/TLS kullanımını etkinleştir
                    client.UseDefaultCredentials = false; // Varsayılan kimlik bilgilerini kullanma
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(smtpUsername, senderNameSurname); // Gönderen adı ve e-postası
                        mail.To.Add(toEmail); // Alıcı e-postası
                        mail.Subject = subject; // E-posta konusu
                        mail.Body = body; // E-posta içeriği
                        mail.IsBodyHtml = false; // HTML içerik ise true yapın

                        await client.SendMailAsync(mail);
                        return true; // E-posta başarıyla gönderildi
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: ", ex.Message);

                TempData["ErrorMessage"] = "E-posta gönderilirken bir hata oluştu: " + ex.Message;

                return false;
            }


        }
    }
}