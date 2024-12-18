using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SlidersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            var sliders = await _context.Sliders
                                        .OrderByDescending(s => s.CreatedDate) 
                                        .ToListAsync();

            if (sliders == null || !sliders.Any())
            {
                sliders = new List<Slider>();
            }

            return PartialView("_SlidersPartial", sliders);
        }

        // POST: Admin/Sliders/AddSliders
        [HttpPost]
        public async Task<IActionResult> AddSliders(IFormFile SliderImg, Slider model)
        {
            if (SliderImg != null && SliderImg.Length > 0)
            {
                var fileName = Path.GetFileName(SliderImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SliderImg.CopyToAsync(stream);
                }

                model.SliderImg = fileName;
            }

            var newSliders = new Slider
            {
                SliderName = model.SliderName,
                SliderTitle = model.SliderTitle,
                SliderImg = model.SliderImg,
                SliderStatus = model.SliderStatus,
                CreatedDate =DateTime.UtcNow,
            };
           
            _context.Sliders.Add(newSliders);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> EditStatus(int sliderId, bool sliderStatus)
        {
            // Tìm slider theo ID
            var slider = await _context.Sliders.FindAsync(sliderId);
            if (slider == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái của slider
            slider.SliderStatus = sliderStatus;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();

            // Trả về kết quả thành công dưới dạng JSON
            return Json(new { success = true });
        }
        // AdminController.cs

        [HttpPost]
        public async Task<IActionResult> DeleteSlider(int sliderId)
        {
            if (sliderId <= 0)
            {
                return Json(new { success = false, message = "ID không hợp lệ." });
            }

            var slider = await _context.Sliders.FindAsync(sliderId);
            if (slider == null)
            {
                return Json(new { success = false, message = "Slider không tìm thấy." });
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

    }

}
