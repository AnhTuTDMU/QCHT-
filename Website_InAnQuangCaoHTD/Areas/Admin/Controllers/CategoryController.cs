using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST: Admin/Category/AddCategory
        [HttpPost]
        public async Task<IActionResult> AddCategory(NavbarViewModel model)
        {
            var newCategory = new Category
            {
                CategoryName = model.CategoryName
            };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        // POST: Admin/Category/DeleteCategory
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // Tìm category theo ID
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                // Nếu không tìm thấy category, trả về thông báo lỗi
                return Json(new { success = false, message = "Không có Ấn Phẩm " });
            }
            _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
                return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories == null || !categories.Any())
            {
                categories = new List<Category>();
            }
            return PartialView("_CategoriesPartial", categories);
        }
    }
}
