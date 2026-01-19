namespace Core_Proje.Areas.Writer.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
