using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ApplicationDbContext _context;
        public ShoppingCartController(IShoppingCartService shoppingCartService, ApplicationDbContext context) : base(context)
        {
            _shoppingCartService = shoppingCartService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "UserAccount");
            }
            LoadUserSessionData();
            var cartItems = await _shoppingCartService.GetCartItemsAsync(userId.Value);
            var cartTotal = await _shoppingCartService.GetCartTotalAsync(userId.Value);

            var viewModel = new ShoppingCartViewModel
            {
                Items = cartItems,
                TotalAmount = cartTotal
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, decimal price)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Người dùng chưa đăng nhập." });
            }

            try
            {
                await _shoppingCartService.AddToCartAsync(userId.Value, productId, quantity, price);
                return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
            }
            catch (Exception ex)
            {            
                return Json(new { success = false, message = ex.Message});
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity, decimal price)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            try
            {
                var result = await _shoppingCartService.UpdateQuantityAsync(userId.Value, productId, quantity, price);

                if (result) // Kiểm tra nếu cập nhật thành công
                {
                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật thất bại" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Người dùng chưa đăng nhập." });
            }

            await _shoppingCartService.RemoveFromCartAsync(userId.Value, productId);

            return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
        }

    }

}
