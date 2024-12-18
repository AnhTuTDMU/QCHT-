using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category ? Category { get; set; }

        [Required]
        [MaxLength(255)]
        public string ? SubCategoryName { get; set; }
        public string? SubCategoryImage { get; set; } 

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
