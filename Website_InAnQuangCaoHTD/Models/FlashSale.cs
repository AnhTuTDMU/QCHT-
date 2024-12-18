using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Website_InAnQuangCaoHTD.Models
{
    public class FlashSale
    {
        [Key]
        public int FlashSaleId { get; set; }

        // Chỉ định khóa ngoại
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public decimal ? FlashSalePrice { get; set; }
        public DateTime FlashSaleStartTime { get; set; }
        public DateTime FlashSaleEndTime { get; set; }
        public bool IsActive { get; set; }
        public string FormattedSalePrice
        {
            get
            {
                return FlashSalePrice.HasValue ? FlashSalePrice.Value.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ" : "Chưa có";
            }
        }
    }
}
