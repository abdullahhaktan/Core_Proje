using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

    }
}
