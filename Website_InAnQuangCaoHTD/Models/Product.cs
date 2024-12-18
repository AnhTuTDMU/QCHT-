using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Website_InAnQuangCaoHTD.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public int SubCategoryID { get; set; }

        [ForeignKey(nameof(SubCategoryID))]
        public SubCategory? SubCategory { get; set; }

        [Required]
        [MaxLength(255)]
        public string? ProductName { get; set; }
        [MaxLength(255)]
        public string? ImageFileName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }

        [MaxLength(255)]
        public string? CategoryProduct { get; set; }

        [MaxLength(100)]
        public string? Size { get; set; }
        public int ? SoldQuantity { get; set; } // Số lượng bán được
        public string FormattedPrice
        {
            get
            {
                return Price.HasValue ? Price.Value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ" : "Chưa có";
            }
        }
        public ICollection<FlashSale> FlashSales { get; set; } = new List<FlashSale>();
        public bool HasActiveFlashSale()
        {
            return FlashSales.Any(fs => fs.IsActive && fs.FlashSaleEndTime > DateTime.Now);
        }
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
    }
}
