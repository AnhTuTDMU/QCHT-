using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookie")]
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IActionResult> Profile()
        {
            LoadUserSessionData();
            // Lấy thông tin người dùng hiện tại
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user == null)
            {
                return NotFound();
            }
            var model = new UsersModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }
        public async Task<IActionResult> OrderSummary(int userId)
        {
            var orderSummary = await _context.Orders
                .Where(o => o.UserId == userId)
                .GroupBy(o => 1)
                .Select(g => new
                {
                    TotalOrders = g.Count(),
                    TotalAmount = g.Sum(o => o.TotalPrice)
                })
                .FirstOrDefaultAsync();

            if (orderSummary == null)
            {
                return Json(new { success = false, message = "No data found" });
            }

            return Json(new { success = true, totalOrders = orderSummary.TotalOrders, totalAmount = orderSummary.TotalAmount });
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            // Lấy thông tin người dùng hiện tại
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user == null)
            {
                return NotFound();
            }
            var model = new UsersModel
            {
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Address = user.Address,
                RoleId = user.RoleId
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var model = new UsersModel
            {
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                Address = user.Address,
                RoleId = user.RoleId
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UsersModel model)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserEmail = model.UserEmail;
            user.Address = model.Address;
            user.RoleId = model.RoleId;

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", user.ProfilePicture);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.ProfilePicture = fileName;
            }
            else
            {
                user.ProfilePicture = string.IsNullOrEmpty(model.ProfilePicture) ? user.ProfilePicture : model.ProfilePicture;
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                data = new
                {
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    UserEmail = user.UserEmail,
                    Address = user.Address,
                    ProfilePicture = user.ProfilePicture,
                    RoleId = user.RoleId
                }
            });
        }


    }

}
