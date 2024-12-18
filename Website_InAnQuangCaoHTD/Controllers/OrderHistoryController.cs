using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class OrderHistoryController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public OrderHistoryController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            LoadUserSessionData();
            // Truy vấn đơn hàng theo UserId
            var ordersQuery = _context.Orders
                .Where(o => o.UserId == userId);
            if (startDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= endDate.Value);
            }

            var orders = ordersQuery
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToList();

            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> GetOderDetails(int orderId)
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
        [HttpPost]
        public IActionResult CancelOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Json(new { success = false });
            }

            // Kiểm tra nếu đơn hàng đã vận chuyển và còn trong vòng 2 ngày
            var daysSinceOrder = (DateTime.Now - order.OrderDate).Days;
            if (order.OrderStatus == 1 || order.OrderStatus == 0 || order.DeliveryDate == null && daysSinceOrder <= 2)
            {
                order.OrderStatus = 3;  // Cập nhật trạng thái thành "Đã Hủy"
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult SuccessfulDelivery(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Json(new { success = false });
            }

            if (order.OrderStatus == 2)
            {
                order.OrderStatus = 5;  // Cập nhật trạng thái thành "Thành công"
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
