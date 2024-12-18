using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsBySubCategory(int subCategoryId)
        {
            var products = await _context.Products
                .Where(p => p.SubCategoryID == subCategoryId)
                .ToListAsync();
            ViewData["SubcategoryID"] = subCategoryId;
            return PartialView("_ProductPartial", products);
        }

        // POST: Admin/Product/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormFile imageFileName, ProductViewModel model)
        {
            // Kiểm tra sự tồn tại của SubCategoryID
            var subCategoryExists = await _context.SubCategories.AnyAsync(sc => sc.SubCategoryID == model.SubCategoryID);
            if (!subCategoryExists)
            {
                return BadRequest("SubCategoryID không hợp lệ.");
            }

            // Xử lý ảnh
            if (imageFileName != null && imageFileName.Length > 0)
            {
                var fileName = Path.GetFileName(imageFileName.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFileName.CopyToAsync(stream);
                }

                model.ImageFileName = fileName;
            }

            var newProduct = new Product
            {
                ProductName = model.ProductName,
                Description = model.Description,
                ImageFileName = model.ImageFileName,
                Price = model.Price,
                CategoryProduct = model.CategoryProduct,
                Size = model.Size,
                SubCategoryID = model.SubCategoryID,
                
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetProductEdit(int id)
        {
            // Lấy sản phẩm từ cơ sở dữ liệu
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                ImageFileName = product.ImageFileName,
                Price = product.Price,
                CategoryProduct = product.CategoryProduct,
                Size = product.Size,
            };

            return Json(model);
        }

        // POST: Admin/Product/SaveProductEdit
        [HttpPost]
        public async Task<IActionResult> SaveProductEdit(ProductViewModel model)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductID == model.ProductID);

            if (product == null)
            {
                return NotFound();
            }

            // Kiểm tra xem người dùng có chọn hình ảnh mới không
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                // Xóa hình ảnh cũ nếu cần thiết
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", product.ImageFileName);
                if (System.IO.File.Exists(oldFilePath) && !string.IsNullOrEmpty(product.ImageFileName))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                // Lưu tệp ảnh mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật tên tệp ảnh trong cơ sở dữ liệu
                product.ImageFileName = fileName;
            }
            else
            {
                // Nếu không chọn hình ảnh mới, giữ nguyên hình ảnh cũ
                product.ImageFileName = string.IsNullOrEmpty(model.ImageFileName) ? product.ImageFileName : model.ImageFileName;
            }

            // Cập nhật thông tin sản phẩm
            product.ProductName = model.ProductName;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryProduct = model.CategoryProduct;
            product.Size = model.Size;


            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult SaveFlashSale(FlashSale flashSale)
        {
            // Tìm sản phẩm theo ProductID
            var product = _context.Products
                .Include(p => p.FlashSales) // Bao gồm các FlashSale liên quan
                .FirstOrDefault(p => p.ProductID == flashSale.ProductId);

            if (product != null)
            {
                // Nếu sản phẩm đã có Flash Sale, kiểm tra xem có Flash Sale nào đang hoạt động không
                var existingFlashSale = product.FlashSales
                    .FirstOrDefault(fs => fs.FlashSaleId == flashSale.FlashSaleId);

                if (existingFlashSale != null)
                {
                    // Cập nhật Flash Sale nếu đã tồn tại
                    existingFlashSale.FlashSalePrice = flashSale.FlashSalePrice;
                    existingFlashSale.FlashSaleStartTime = flashSale.FlashSaleStartTime;
                    existingFlashSale.FlashSaleEndTime = flashSale.FlashSaleEndTime;
                    existingFlashSale.IsActive = flashSale.IsActive;
                }
                else
                {
                    // Thêm mới Flash Sale
                    product.FlashSales.Add(flashSale);
                    _context.FlashSales.Add(flashSale);
                }

                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại." });
        }
        [HttpGet]
        public IActionResult CheckActiveFlashSale(int productId)
        {
            // Kiểm tra xem productId có hợp lệ không
            if (productId <= 0)
            {
                return Json(new { success = false, message = "ID sản phẩm không hợp lệ." });
            }

            var product = _context.Products
                .Include(p => p.FlashSales)
                .FirstOrDefault(p => p.ProductID == productId);

            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            // Không cần kiểm tra và cập nhật IsActive, vì việc này đã được thực hiện bởi Task Scheduler

            // Kiểm tra xem sản phẩm có Flash Sale nào đang hoạt động hay không
            var hasActiveFlashSale = product.HasActiveFlashSale();

            return Json(new { success = true, hasActiveFlashSale });
        }
    }

}
