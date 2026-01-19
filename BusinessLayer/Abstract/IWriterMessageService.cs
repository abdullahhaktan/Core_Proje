using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IWriterMessageService
    {
        List<WriterMessage> GetListInbox(string mail);
        List<WriterMessage> GetListInDraft(string mail);
        List<WriterMessage> GetListSendbox(string mail);
        List<WriterMessage> GetListInTrash(string mail);
        void SendMail(string mail, string subject, string message);
        void MessageAdd(WriterMessage message);
        WriterMessage GetByID(int id);
        void MessageDelete(WriterMessage message);
        void MessageUpdate(WriterMessage message);
    }
}
