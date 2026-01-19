using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityLayer.Concrete
{
    public class Portfolio
    {
        [Key]
        public int PortfolioID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrl1 { get; set; }
        public string ProjectUrl { get; set; }
        public string Platform { get; set; }
        public string Price { get; set; }
        public bool Status { get; set; }
        public int Value { get; set; }
        public string User { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; } // Bu sadece geçici form verisi için kullanılacak,

        [NotMapped]
        public IFormFile Image1 { get; set; } // Bu sadece geçici form verisi için kullanılacak


        public string screenShotImageUrl { get; set; }
        public string screenShotImageUrl1 { get; set; }
        public string screenShotImageUrl2 { get; set; }

        [NotMapped]
        public IFormFile screenShotImage { get; set; } // Bu sadece geçici form verisi için kullanılacak,

        [NotMapped]
        public IFormFile screenShotImage1 { get; set; } // Bu sadece geçici form verisi için kullanılacak

        [NotMapped]
        public IFormFile screenShotImage2 { get; set; } // Bu sadece geçici form verisi için kullanılacak

    }
}