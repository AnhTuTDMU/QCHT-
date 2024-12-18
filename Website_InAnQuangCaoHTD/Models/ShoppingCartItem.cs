using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_InAnQuangCaoHTD.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int ShoppingCartItemId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }
        public Product ? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [ForeignKey(nameof(ShoppingCartId))]
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UsersModel ? User { get; set; }
    }
}
