using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Area.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            var userName = HttpContext.Session.GetString("Username");
            var useremail = HttpContext.Session.GetString("Usergmail");
            ViewBag.Username = userName;
            ViewBag.Useremail = useremail;
            if (categories == null || !categories.Any())
            {
                categories = new List<Category>();
            }

            var sliders = _context.Sliders.ToList();
            if (sliders == null || !sliders.Any())
            {
                sliders = new List<Slider>();
            }

            ViewData["Categories"] = categories;
            ViewData["Sliders"] = sliders;

            return View();
        }
        public async Task<IActionResult> LoadDashboard()
        {
            // Đếm số lượng sản phẩm
            ViewBag.ProductCount = await _context.Products.CountAsync();

            // Đếm số lượng khách hàng
            ViewBag.CustomerCount = await _context.Users.Where(o=> o.RoleId == 2).CountAsync();

            // Đếm số lượng đơn hàng
            ViewBag.OrderCount = await _context.Orders.CountAsync();

            // Đếm số lượng doanh thu trong tháng
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            ViewBag.MonthlyRevenue = GetMonthlyRevenue(currentYear, currentMonth);
            return PartialView("_DashboardPartial"); 
        }
        public decimal GetMonthlyRevenue(int year, int month)
        {
            var totalRevenue = _context.Orders
           .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month && o.IsPaid)
           .Sum(o => (decimal?)o.TotalPrice) ?? 0; 

            return totalRevenue;
        }
    }

}
