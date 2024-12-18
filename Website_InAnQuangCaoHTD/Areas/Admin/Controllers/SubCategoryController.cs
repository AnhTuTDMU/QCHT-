using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{

    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subCategories = await _context.SubCategories
                .Where(sc => sc.CategoryID == categoryId)
                .ToListAsync();
            ViewData["CategoryID"] = categoryId;
            return PartialView("_SubCategoriesPartial", subCategories);
        }
        // POST: Admin/SubCategory/AddSubCategory
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(IFormFile SubCategoryImg,SubCategoryViewModel model)
        {
            // Xử lý ảnh
            if (SubCategoryImg != null && SubCategoryImg.Length > 0)
            {
                var fileName = Path.GetFileName(SubCategoryImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SubCategoryImg.CopyToAsync(stream);
                }

                model.SubCategoryImage = fileName;
            }
            var subCategory = new SubCategory
            {
                SubCategoryName = model.SubCategoryName,
                SubCategoryImage = model.SubCategoryImage,
                CategoryID = model.CategoryID              
            };

            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
       
        [HttpGet]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(sc => sc.SubCategoryID == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            var model = new
            {
                subCategoryId = subCategory.SubCategoryID,
                subCategoryName = subCategory.SubCategoryName,
                subCategoryImage = subCategory.SubCategoryImage
            };

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSubCategory(SubCategory model)
        {
            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(p => p.SubCategoryID == model.SubCategoryID);

            if (subCategory == null)
            {
                return NotFound();
            }

            // Kiểm tra xem người dùng có chọn hình ảnh mới không
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                // Xóa hình ảnh cũ nếu cần thiết
                if (!string.IsNullOrEmpty(subCategory.SubCategoryImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", subCategory.SubCategoryImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Lưu tệp ảnh mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật tên tệp ảnh trong cơ sở dữ liệu
                subCategory.SubCategoryImage = fileName;
            }
            else
            {
                // Nếu không chọn hình ảnh mới, giữ nguyên hình ảnh cũ
                subCategory.SubCategoryImage = string.IsNullOrEmpty(model.SubCategoryImage) ? subCategory.SubCategoryImage : model.SubCategoryImage;
            }

            // Cập nhật thông tin subcategory
            subCategory.SubCategoryName = model.SubCategoryName;

            _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
    }
}
