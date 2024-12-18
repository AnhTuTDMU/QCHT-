using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Website_InAnQuangCaoHTD.Helpers;
using Microsoft.Extensions.Options;

namespace Website_InAnQuangCaoHTD.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Area("Admin")]
    public class ManageOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSettings _emailSettings;
        public ManageOrdersController(ApplicationDbContext context, IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }
        public IActionResult ManageOrders()
        {
            var orders = _context.Orders.Include(o => o.Users).Include(o => o.OrderDetails).ToList();
            return View(orders);
        }
        // Lấy đơn hàng theo trạng thái
        public async Task<IActionResult> GetOrdersByStatus(int status)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderStatus == status)
                .Include(o => o.Users)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            return PartialView("_OrdersListPartial", orders);
        }
        // Tất cả đơn hàng
        public async Task<IActionResult> GetOrderAll()
        {
            var pendingOrders = await _context.Orders
                .Where(o => o.OrderStatus == 1 || o.OrderStatus == 2 || o.OrderStatus == 3 || o.OrderStatus == 0)
                .Include(o => o.Users)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();

            return PartialView("_OrdersListAllPartial", pendingOrders);
        }


        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Json(new { success = false });
            }

            // Chỉ cập nhật trạng thái nếu đơn hàng đang ở trạng thái "Đã Xử Lý"
            if (order.OrderStatus == 1)
            {
                order.OrderStatus = 2;  // Cập nhật trạng thái thành "Đang Giao Hàng"
                _context.SaveChanges(); 
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return PartialView("_OrderDetailsPartial", order);
        }
        public decimal GetMonthlyRevenue(int year, int month)
        {
            var totalRevenue = _context.Orders
           .Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month && o.IsPaid)
           .Sum(o => (decimal?)o.TotalPrice) ?? 0; 

            return totalRevenue;
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int orderId, DateTime deliveryDate)
        {
            var order = await _context.Orders
                .Include(o=> o.Users)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order != null)
            {
                order.OrderStatus = 1; // Đã xử lý
                order.DeliveryDate = deliveryDate; // Cập nhật ngày giao hàng
                                                   // Cập nhật số lượng bán được cho từng sản phẩm trong đơn hàng
                foreach (var orderItem in order.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(orderItem.ProductId);
                    if (product != null)
                    {
                        product.SoldQuantity = (product.SoldQuantity ?? 0) + orderItem.Quantity; // Cộng dồn số lượng đã bán
                        try
                        {
                            _context.Products.Update(product);                         
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi cập nhật sản phẩm {product.ProductID}: {ex.Message}");
                        }
                    }
                }

                _context.Update(order);
                await _context.SaveChangesAsync();
                 await SendNotificationToManager(order);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        // Cập nhật trạng thái thanh toán
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(int orderId, bool isPaid)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsPaid = isPaid;

                _context.Update(order);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        private async Task SendNotificationToManager(Order order)
        {     
            var subject = "Thông báo: Đơn hàng đã được xác nhận";
            var message = $@"
              <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <h3 style='color: #0056b3;'>Kính gửi Khách hàng,</h3>
                <p>Đơn hàng có mã: <strong style='color: #ff5722;'>{order.OrderId}</strong> đã được xác nhận và sẽ giao vào ngày 
                    <strong style='color: #ff5722;'>{order.DeliveryDate?.ToString("dd/MM/yyyy")}</strong>.</p>  
                <p>Tổng số tiền: <strong style='color: #4caf50;'>{order.TotalPrice.ToString("N0", new System.Globalization.CultureInfo("vi-VN"))} VNĐ</strong></p>
                <p>Trạng thái: <span style='color: green; font-weight: bold;'>Đã xử lý</span>.</p>
                <p>Đơn hàng sẽ được giao theo đúng ngày dự kiến của chúng tôi.</p>
                <p>Trân trọng cảm ơn!</p>
                <p style='margin-top: 20px;'>Đội ngũ hỗ trợ Quảng Cáo HTĐ</p>
                <hr style='border: 1px solid #ddd;' />
                <footer style='font-size: 12px; color: #888;'>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email support@yourwebsite.com.</footer>
            </div>";
            // Cấu hình SMTP
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("Người nhận", order.Users.UserEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);

                    // Gửi email
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
