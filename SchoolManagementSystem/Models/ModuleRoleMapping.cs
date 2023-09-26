using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace SchoolManagementSystem.Models
{
    public class ModuleRoleMapping
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
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool StatusFlag { get; set; }


    }
}
