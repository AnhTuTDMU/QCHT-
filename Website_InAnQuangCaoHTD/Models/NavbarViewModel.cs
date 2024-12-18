namespace Website_InAnQuangCaoHTD.Models
{
    public class NavbarViewModel
    {
        public int CategoryID { get; set; }
        public string ? CategoryName { get; set; }
        public List<SubCategoryViewModel> ? SubCategories { get; set; }
    }

    public class SubCategoryViewModel
    {
        public int SubCategoryID { get; set; }
        public int CategoryID { get; set; }
        public string ? SubCategoryName { get; set; }
        public string ? SubCategoryImage{ get; set; }

        public List<ProductViewModel> ? Products { get; set; }
    }
    public class FlashSaleViewModel
    {
        public int FlashSaleId { get; set; }
        public DateTime FlashSaleStartTime { get; set; }
        public DateTime FlashSaleEndTime { get; set; }
        public List<ProductViewModel> ? Products { get; set; }
    }
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public int SubCategoryID { get; set; }
        public string ? ProductName { get; set; }
        public string ? ImageFileName { get; set; }
        public string ? Description { get; set; }
        public decimal? Price { get; set; }
        public string? CategoryProduct { get; set; }
        public string? Size { get; set; }
        public decimal OriginalPrice { get; set; }  
        public decimal FlashSalePrice { get; set; }


    }

}
