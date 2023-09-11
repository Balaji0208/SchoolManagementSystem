using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models.DTO
{
    public class RoleDetailsDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool StatusFlag { get; set; }

        
    }
}
