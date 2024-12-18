using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticalController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> BestSellingProducts()
        {
            // Lấy danh sách sản phẩm và số lượng bán
            var products = await _context.Products
                .Where(p=> p.SoldQuantity > 0)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.SoldQuantity
                })
                .OrderByDescending(p => p.SoldQuantity) // Sắp xếp theo số lượng bán
                .ToListAsync();

            return PartialView("_BestSellingPartial", products);
        }
        public IActionResult BestSellingProducts_SubCategory()
        {
          
            var products =  _context.Products
                .Include(p=> p.SubCategory)
                .Where(p => p.SoldQuantity > 0) 
                .Select(p => new
                {
                    p.ProductName,
                    p.SoldQuantity,
                    SubCategoryName = p.SubCategory.SubCategoryName
                })
                .OrderByDescending(p => p.SoldQuantity) 
                .ToList(); 

            return Json(products);
        }
        public IActionResult BestSellingProducts_Category()
        {
            var products = _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)  // Đảm bảo có Category
                .Where(p => p.SoldQuantity > 0)
                .Select(p => new
                {
                    p.ProductName,
                    p.SoldQuantity,
                    SubCategoryName = p.SubCategory.SubCategoryName,
                    CategoryName = p.SubCategory.Category != null ? p.SubCategory.Category.CategoryName : "Không có danh mục" // Kiểm tra null
                })
                .OrderByDescending(p => p.SoldQuantity)
                .ToList();

            return Json(products);
        }


    }
}
