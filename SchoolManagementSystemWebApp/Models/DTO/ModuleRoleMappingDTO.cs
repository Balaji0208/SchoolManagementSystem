using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class ModuleRoleMappingDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RoleMapId { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
       
        public ModuleDTO? Module { get; set; }
       

        [ForeignKey("RoleDetails")]
        public int RoleId { get; set; }
        public RoleDetailsDTO? RoleDetails { get; set; }
        public bool StatusFlag { get; set; }
    }
}
