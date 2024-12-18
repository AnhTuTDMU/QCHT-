using Microsoft.EntityFrameworkCore;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCartItem>> GetCartItemsAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity, decimal price);
        Task RemoveFromCartAsync(int userId, int productId);
        Task<decimal> GetCartTotalAsync(int userId);
        Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity, decimal price);
        Task ClearCartAsync(int userId);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingCartItem>> GetCartItemsAsync(int userId)
        {
            return await _context.ShoppingCartItems
                .Include(item => item.Product)
                .Where(item => item.UserId == userId)
                .ToListAsync();
        }


        public async Task AddToCartAsync(int userId, int productId, int quantity, decimal price)
        {
            // Kiểm tra và tạo giỏ hàng nếu chưa có
            var shoppingCart = await _context.ShoppingCarts
                                              .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(shoppingCart);
                await _context.SaveChangesAsync(); // Lưu thay đổi để lấy ShoppingCartId
            }

            // Thêm hoặc cập nhật mục giỏ hàng
            var cartItem = await _context.ShoppingCartItems
                                          .FirstOrDefaultAsync(item => item.ProductId == productId && item.ShoppingCartId == shoppingCart.ShoppingCartId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    throw new ArgumentException("Sản phẩm không tồn tại");

                _context.ShoppingCartItems.Add(new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = price,
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                    UserId = userId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity, decimal price)
        {
            var cartItem = await _context.ShoppingCartItems
                                          .FirstOrDefaultAsync(item => item.ProductId == productId && item.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.Price = price;
                await _context.SaveChangesAsync();
                return true;
            }
            return false; // Trả về false nếu sản phẩm không được tìm thấy
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cartItem = await _context.ShoppingCartItems
                                          .FirstOrDefaultAsync(item => item.ProductId == productId && item.UserId == userId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetCartTotalAsync(int userId)
        {
            var items = await _context.ShoppingCartItems
                .Where(item => item.UserId == userId)
                .ToListAsync();

            return items.Sum(item => item.Price * item.Quantity);
        }


        public async Task ClearCartAsync(int userId)
        {
            // Lấy tất cả các mặt hàng trong giỏ hàng của người dùng
            var cartItems = await _context.ShoppingCartItems
                .Where(item => item.UserId == userId)
                .ToListAsync();

            // Xóa tất cả các mặt hàng khỏi giỏ hàng
            _context.ShoppingCartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }

}
