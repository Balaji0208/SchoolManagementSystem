using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class RoleDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [MaxLength(20)]
        [Required]
        public string RoleName { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool StatusFlag { get; set; }
    }
}
