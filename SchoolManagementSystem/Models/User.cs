using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
       
        [ForeignKey("RoleMaster")]
        public int RoleId { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set;}

        public DateTime UpdatedDate { get; set;}
        public int UpdatedBy { get; set; }
        public bool StatusFlag { get; set; }
    }
}
