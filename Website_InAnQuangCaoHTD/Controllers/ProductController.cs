using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context) : base(context) { 
            _context = context;
        }

        // GET: /product/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewData["NavbarData"] = await GetNavbarDataAsync();
            LoadUserSessionData();
             var product = await _context.Products
               .Include(p => p.FlashSales)
               .FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // GET: /product/subcategory
        [HttpGet]
        public async Task<IActionResult> GetAllSubCategory(int id)
        {
            ViewData["NavbarData"] = await GetNavbarDataAsync();
            LoadUserSessionData();
               var subCategory = await _context.SubCategories
                .Include(sc => sc.Products) 
                .FirstOrDefaultAsync(sc => sc.SubCategoryID == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }
        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = string.IsNullOrEmpty(searchTerm)
                ? await _context.Products.ToListAsync()
                : await _context.Products
                      .Where(p => p.ProductName.Contains(searchTerm))
                      .ToListAsync();

            return View(products); 
        }


    }
}
