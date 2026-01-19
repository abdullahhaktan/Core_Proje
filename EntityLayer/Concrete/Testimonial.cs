using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Concrete
{
    public class Testimonial
    {
        [Key]
        public int TestimonialID { get; set; }
        public string ClientName { get; set; }
        public string Company { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string User { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; } // Bu sadece geçici form verisi için kullanılacak,
    }
}
