using Microsoft.AspNetCore.Mvc;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class ConstructionBookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ConstructionBookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateConstructionBooking(ConstructionBooking booking)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (!userId.HasValue)
                {
                    return Json(new { success = false, message = "Chưa đăng nhập." });
                }

                // Kiểm tra các giá trị của booking
                if (string.IsNullOrEmpty(booking.BookingDate.ToString()) || string.IsNullOrEmpty(booking.Description) || string.IsNullOrEmpty(booking.Address))
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }
                if (booking.BookingDate < DateTime.Today)
                {
                    return Json(new { success = false, message = "Ngày đặt không được nhỏ hơn ngày hiện tại." });
                }
                bool isDateBooked = _context.ConstructionBookings
                               .Any(cb => cb.BookingDate == booking.BookingDate);
                if (isDateBooked)
                {
                    // Trả về lỗi nếu ngày đã đặt
                    return Json(new { success = false, message = "Ngày này đã được đặt. Vui lòng chọn ngày khác." });
                }
                else
                {
                    var newBooking = new ConstructionBooking
                    {
                        UserId = userId.Value,
                        BookingDate = booking.BookingDate,
                        Description = booking.Description,
                        Address = booking.Address,
                        IsConfirmed = 0
                    };

                    _context.ConstructionBookings.Add(newBooking);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Đặt lịch thành công! Chúng tôi sẽ liên hệ với bạn qua zalo hoặc số điện thoại" });
                }

            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Đã xảy ra lỗi khi đặt lịch." });
            }
        }
        [HttpGet]
        public IActionResult GetBookedDates()
        {
            // Lấy danh sách các ngày đã đặt
            var bookedDates = _context.ConstructionBookings
                                      .Select(cb => cb.BookingDate.ToString("yyyy-MM-dd"))
                                      .ToList();

            return Json(new { bookedDates });
        }



    }
}
