using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> GetCustomer()
        {
            var customers = await _context.Users.Where(cs => cs.RoleId == 2).ToListAsync();
            return PartialView("_CustomerList", customers);
        }
        [HttpGet]
        public async Task<IActionResult> GetNewCustomers(int? month, int? year)
        {
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            var customers = await _context.Users
                .Where(u => u.RoleId == 2 &&
                            u.DateCreated.Month == currentMonth &&
                            u.DateCreated.Year == currentYear)
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.UserEmail,
                    u.PhoneNumber,
                    u.Address,
                    ProfilePicture = string.IsNullOrEmpty(u.ProfilePicture) ? "userDefault.jpg" : u.ProfilePicture,
                    CreatedDate = u.DateCreated.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Json(customers);
        }

        public IActionResult GetUserStatistics()
        {
            var userStatistics = _context.Users.Where(us=> us.RoleId == 2)
                .GroupBy(u => new { u.DateCreated.Year, u.DateCreated.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .ToList();

            return Json(userStatistics);
        }
    }
}
