using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) : base(context) 
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            LoadUserSessionData();
            ViewData["Subcategory"] = await GetSubcategoryAsync();
            ViewData["Sliders"] = await GetSlidersAsync();
            ViewData["NavbarData"] = await GetNavbarDataAsync();
            ViewData["FlashSales"] = await GetFlashSales();
            ViewData["PopularProducts"] = await BestSellingProducts();
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
