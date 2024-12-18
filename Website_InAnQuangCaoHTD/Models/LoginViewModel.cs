using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(50, ErrorMessage = "Email không được quá 50 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "Mật khẩu không được quá 50 ký tự")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Nhớ mật khẩu")]
        public bool RememberMe { get; set; }
        public string SiteKey { get; set; } = "6LeVDTcqAAAAAFh4VP7on6yGTi29Y4g8YI5CHJL6";
    }
}
