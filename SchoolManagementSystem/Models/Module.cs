using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace SchoolManagementSystem.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        public string Menus { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }   
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set;}
        public bool StatusFlag { get; set; }    
    }
}
