using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class PortfolioValidator : AbstractValidator<Portfolio>
    {
        public PortfolioValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Proje adı boş geçilemez");
            //RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel alanı boş geçilemez");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş geçilemez");
            RuleFor(x => x.Value).NotEmpty().WithMessage("Değer alanı boş geçilemez");
            RuleFor(x => x.Name).MinimumLength(5).WithMessage("Proje adı en az 5 karakterden oluşmak zorundadır");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Proje adı en fazla 100 karakterden oluşmak zorundadır");
        }
    }
}
