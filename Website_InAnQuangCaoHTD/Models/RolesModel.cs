using System.ComponentModel.DataAnnotations;

namespace Website_InAnQuangCaoHTD.Models
{
    public class RolesModel
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;
        public List<UsersModel> Users { get; set; } = new List<UsersModel>();
    }
}
