using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_InAnQuangCaoHTD.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Ngày đặt hàng")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày giao hàng")]
        public DateTime ? DeliveryDate { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public UsersModel Users { get; set; }
        public int OrderStatus { get; set; } = 0;
        public bool IsPaid { get; set; } = false;
    }

}
