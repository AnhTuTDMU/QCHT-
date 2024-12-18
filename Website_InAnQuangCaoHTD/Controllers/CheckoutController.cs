using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Helpers;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vpnPayService;
        public CheckoutController(IShoppingCartService shoppingCartService, ApplicationDbContext context, IVnPayService vnPayService) : base(context)
        {
            _shoppingCartService = shoppingCartService;
            _context = context;
            _vpnPayService = vnPayService;
        }

        // Hiển thị trang Checkout
        public async Task<IActionResult> Index()
        {
            LoadUserSessionData();
             var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _shoppingCartService.GetCartItemsAsync(userId.Value);
            var totalAmount = await _shoppingCartService.GetCartTotalAsync(userId.Value);
            var shoppingCart = new ShoppingCartViewModel
            {
                Items = cartItems ?? new List<ShoppingCartItem>(),  
                TotalAmount = totalAmount
            };

            var model = new CheckoutViewModel
            {
                ShoppingCart = shoppingCart,
                UserId = userId.Value,
                TotalAmount = totalAmount
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model, string PaymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                // Nếu không có userId, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            model.UserId = userId.Value;

            // Tính tổng số tiền của giỏ hàng
            var totalAmount = await _shoppingCartService.GetCartTotalAsync(userId.Value);
            model.TotalAmount = totalAmount;

            if (!ModelState.IsValid)
            {
                try
                {
                    // Lấy các sản phẩm trong giỏ hàng của người dùng
                    var cartItems = await _shoppingCartService.GetCartItemsAsync(userId.Value);

                    // Tạo đơn hàng mới từ dữ liệu giỏ hàng
                    var order = new Order
                    {
                        UserId = model.UserId,
                        TotalPrice = model.TotalAmount,
                        Address = model.Address,
                        PaymentMethod = model.PaymentMethod,
                        OrderDate = DateTime.Now,
                        DeliveryDate = null,
                        OrderStatus = 0,
                        OrderDetails = cartItems.Select(item => new OrderDetail
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        }).ToList()
                    };

                    // Thêm đơn hàng vào cơ sở dữ liệu
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    // Lấy OrderId sau khi lưu
                    var orderId = order.OrderId; //  OrderId là khóa chính tự động tăng
                    HttpContext.Session.SetInt32("OrderId", order.OrderId);

                    if (PaymentMethod == "VNPAY")
                    {
                        // Tạo mô hình thanh toán VNPAY
                        var VnPayModel = new VnPaymentRequestModel
                        {
                            Amount = (double)model.TotalAmount,
                            CreatedDate = DateTime.Now,
                            Description = $"{model.Address}",
                            OrderId = orderId // Truyền OrderId vào model
                        };

                        // Tạo URL thanh toán với VNPAY
                        var paymentUrl = _vpnPayService.CreatePaymentUrl(HttpContext, VnPayModel);
                    
                        return Redirect(paymentUrl);
                    }

                    // Xóa giỏ hàng sau khi đặt hàng thành công
                    await _shoppingCartService.ClearCartAsync(userId.Value);

                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và hiển thị thông báo lỗi cho người dùng
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi xử lý đơn hàng.");
                }
            }
            return View("Index", model);
        }

        public IActionResult PaymentFail()
        {
            return View();
        }
        public async Task<IActionResult> PaymentCalBack()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPAY: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            // Lấy OrderId từ session
            var orderId = HttpContext.Session.GetInt32("OrderId");

            if (!orderId.HasValue)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("PaymentFail");
            }

            // Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
            var order = await _context.Orders.FindAsync(orderId.Value);
            if (order != null)
            {
                order.IsPaid = true; // Thanh toán thành công
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }

            // Xóa giỏ hàng của người dùng
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                await _shoppingCartService.ClearCartAsync(userId.Value);
            }
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            LoadUserSessionData();
            return View();
        }
    }
}
