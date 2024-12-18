using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ? CategoryName { get; set; }
        [MaxLength(255)]

        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
