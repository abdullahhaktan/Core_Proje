using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class ToDoList
    {
        [Key]
        public int ID { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public string User { get; set; }
    }
}
