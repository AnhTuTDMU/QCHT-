using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class UsersModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [Display(Name = "Họ tên")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Hình ảnh")]
        public string ProfilePicture { get; set; } = string.Empty;
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Display(Name = "Chọn ảnh")]
        [NotMapped]
        public IFormFile? FrontImg { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        // Khóa ngoại tới Role
        [Display(Name = "Chức vụ")]
        public int RoleId { get; set; }
        public RolesModel? Role { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ConstructionBooking> ConstructionBookings { get; set; }
    }
}
