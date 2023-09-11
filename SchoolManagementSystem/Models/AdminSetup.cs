using System.Diagnostics.Contracts;

namespace SchoolManagementSystem.Models
{
    public class AdminSetup
    {
        public int id { get; set; }
        public string Menu { get; set; }
        public int ParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }   
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set;}
        public bool StatusFlag { get; set; }    
    }
}
