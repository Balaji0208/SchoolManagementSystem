using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models.DTO
{
    public class ModuleRoleMappingDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RoleMapId { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        public Module? Module { get; set; }

        [ForeignKey("RoleDetails")]
        public int RoleId { get; set; }
        public RoleDetails? RoleDetails { get; set; }
        public bool StatusFlag { get; set; }
    }
}
