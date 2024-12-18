using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class Slider
    {
        [Key]
        public int SliderId {  get; set; }
        [Required]
        public string ? SliderImg {  get; set; }
        [Required]
        public string ? SliderName { get; set; }
        [Required]
        public string ? SliderTitle { get; set; }
        [Required]
        public bool SliderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
