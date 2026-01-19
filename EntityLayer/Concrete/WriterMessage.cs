using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class WriterMessage
    {
        [Key]
        public int WriterMessageID { get; set; }

        [StringLength(50)]
        public string SenderMail { get; set; }

        [StringLength(50)]
        public string ReceiverMail { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }

        public string Subject { get; set; }

        public string MessageContent { get; set; }

        public DateTime MessageDate { get; set; }

        public byte MessageStatu { get; set; }
    }
}
