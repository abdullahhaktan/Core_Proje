using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    public class WriterMessageManager : IWriterMessageService
    {
        IWriterMessageDal _writerMessageDal;

        public WriterMessageManager(IWriterMessageDal writerMessageDal)
        {
            _writerMessageDal = writerMessageDal;
        }

        public WriterMessage GetByID(int id)
        {
            return _writerMessageDal.GetByID(id);
        }

        public List<WriterMessage> GetListInbox(string mail)
        {
            return _writerMessageDal.GetList().Where(m => m.ReceiverMail == mail && m.MessageStatu == 2).ToList();
        }

        public List<WriterMessage> GetListInDraft(string mail)
        {
            return _writerMessageDal.GetList().Where(m => m.SenderMail == mail && m.MessageStatu == 1).ToList();
        }

        public List<WriterMessage> GetListInTrash(string mail)
        {
            return _writerMessageDal.GetList().Where(m => m.SenderMail == mail && m.MessageStatu == 0).ToList();
        }

        public List<WriterMessage> GetListSendbox(string mail)
        {
            return _writerMessageDal.GetList().Where(m => m.SenderMail == mail && m.MessageStatu == 2).ToList();
        }

        public void MessageAdd(WriterMessage message)
        {
            _writerMessageDal.Insert(message);
        }

        public void MessageDelete(WriterMessage message)
        {
            _writerMessageDal.Delete(message);
        }

        public void MessageUpdate(WriterMessage message)
        {
            _writerMessageDal.Update(message);
        }

        public void SendMail(string mail, string subject, string message)
        {
            var mail1 = "*****";
            var password = "******";
        }
    }
}