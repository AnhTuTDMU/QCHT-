using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class CheckoutViewModel
    {
        public int UserId { get; set; }
        public ShoppingCartViewModel ShoppingCart { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
