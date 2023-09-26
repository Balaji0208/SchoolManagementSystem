using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class ModuleDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        public string Menus { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }  
        public string Url { get; set; }
        public bool StatusFlag { get; set; }
    }
}
