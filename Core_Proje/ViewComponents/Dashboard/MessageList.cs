using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Proje.ViewComponents.Dashboard
{
    public class MessageList : ViewComponent
    {
        private readonly UserManager<WriterUser> _userManager;
        public MessageList(UserManager<WriterUser> userManager)
        {
            _userManager = userManager;
        }

        WriterMessageManager messageManager = new WriterMessageManager(new EfWriterMessageDal());
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = messageManager.GetListInbox(user.Email).Take(5).ToList();
            return View(values);
        }
    }
}
