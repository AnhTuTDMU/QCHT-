using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class ConstructionBookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ConstructionBookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetUnprocessedBookings()
        {
            var unprocessedBookings = await _context.ConstructionBookings
                                                    .Where(cb => cb.IsConfirmed == 0)
                                                    .Include(cb => cb.Users)
                                                    .Select(cb => new 
                                                    {
                                                        ConstructionBookingId = cb.ConstructionBookingId,
                                                        BookingDate = cb.BookingDate,
                                                        Description = cb.Description,
                                                        Address = cb.Address,
                                                        UserName = cb.Users.UserName,
                                                        Email = cb.Users.UserEmail,
                                                        PhoneNumber = cb.Users.PhoneNumber
                                                    })
                                                    .ToListAsync();

            return PartialView("_UnprocessedBookings", unprocessedBookings);
        }
        [HttpGet]
        public async Task<IActionResult> GetProcessedBookings()
        {
            var processedBookings = await _context.ConstructionBookings
                                                   .Where(cb => cb.IsConfirmed == 1)
                                                   .Include(cb => cb.Users)
                                                   .Select(cb => new 
                                                   {
                                                       ConstructionBookingId = cb.ConstructionBookingId,
                                                       BookingDate = cb.BookingDate,
                                                       Description = cb.Description,
                                                       Address = cb.Address,
                                                       UserName = cb.Users.UserName,
                                                       Email = cb.Users.UserEmail,
                                                       PhoneNumber = cb.Users.PhoneNumber
                                                   })
                                                   .ToListAsync();

            return PartialView("_ProcessedBookings", processedBookings);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.ConstructionBookings.FindAsync(id);
            if (booking == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đặt lịch." });
            }

            booking.IsConfirmed = 1;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đặt lịch đã được xác nhận." });
        }


    }
}
