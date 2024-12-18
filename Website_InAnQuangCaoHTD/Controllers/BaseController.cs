using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        protected BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected async Task<List<NavbarViewModel>> GetNavbarDataAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Products)
                .Select(c => new NavbarViewModel
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    SubCategories = c.SubCategories.Select(sc => new SubCategoryViewModel
                    {
                        SubCategoryID = sc.SubCategoryID,
                        SubCategoryName = sc.SubCategoryName,
                        Products = sc.Products.Select(p => new ProductViewModel
                        {
                            ProductID = p.ProductID,
                            ProductName = p.ProductName,
                            ImageFileName = p.ImageFileName,
                            Description = p.Description,                       
                        }).ToList()
                    }).ToList()
                }).ToListAsync();
        }
        protected async Task<List<Slider>> GetSlidersAsync()
        {
            return await _context.Sliders
                .Where(s => s.SliderStatus)
                .ToListAsync();
        }
        protected async Task<List<SubCategory>> GetSubcategoryAsync()
        {
            return await _context.SubCategories.ToListAsync();
        }
        public async Task<List<FlashSaleViewModel>> GetFlashSales()
        {
            var flashSales = await _context.FlashSales
                .Include(fs => fs.Product)
                .Where(fs => fs.IsActive && fs.FlashSaleEndTime > DateTime.Now)
                .Select(fs => new FlashSaleViewModel
                {
                    FlashSaleId = fs.FlashSaleId,
                    FlashSaleStartTime = fs.FlashSaleStartTime,
                    FlashSaleEndTime = fs.FlashSaleEndTime,
                    Products = new List<ProductViewModel>
                    {
                new ProductViewModel
                {
                    ProductID = fs.Product.ProductID,
                    ProductName = fs.Product.ProductName,
                    ImageFileName = fs.Product.ImageFileName,
                    Description = fs.Product.Description,
                    OriginalPrice = fs.Product.Price ?? 0,
                    FlashSalePrice = fs.FlashSalePrice ?? 0,
                }
                    }
                }).ToListAsync();

            return flashSales;
        }
        protected async Task<List<Product>> BestSellingProducts()
        {
            var bestSellingProducts = await _context.Products
                .Where(p => p.SoldQuantity != null && p.SoldQuantity > 0) 
                .OrderByDescending(p => p.SoldQuantity) 
                .ToListAsync();

            return bestSellingProducts;
        }

        protected void LoadUserSessionData()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var username = HttpContext.Session.GetString("Username");
            var useremail = HttpContext.Session.GetString("Usergmail");
            var rolename = HttpContext.Session.GetString("Rolename");

            ViewBag.userid = userid;
            ViewBag.Username = username;
            ViewBag.Useremail = useremail;
            ViewBag.Rolename = rolename;
          
        }
      
    }
}
