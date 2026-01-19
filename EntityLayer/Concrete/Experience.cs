using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Concrete
{
    public class Experience
    {
        [Key]
        public int ExprerienceID { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public string User { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; } // Bu sadece geçici form verisi için kullanılacak
    }
}
