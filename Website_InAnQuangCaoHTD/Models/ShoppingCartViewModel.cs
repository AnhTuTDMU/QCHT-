namespace Website_InAnQuangCaoHTD.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public decimal TotalAmount { get; set; }
    }

}
