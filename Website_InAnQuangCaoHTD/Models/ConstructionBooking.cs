using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_InAnQuangCaoHTD.Models
{
    public class ConstructionBooking
    {
        [Key]
        public int ConstructionBookingId { get; set; } // ID đặt lịch thi công
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        public DateTime BookingDate { get; set; } // Ngày đặt lịch
        public string Description { get; set; } // Mô tả chi tiết thi công
        public string Address { get; set; } // Địa chỉ
        public int IsConfirmed { get; set; } // Trạng thái
        public UsersModel Users { get; set; }
    }
}
