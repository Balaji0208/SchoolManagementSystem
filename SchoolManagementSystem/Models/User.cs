using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [ForeignKey("Register")]
        public int registerId { get; set; }
        public Register? Register { get; set; }

        [ForeignKey("RoleDetails")]
        public int RoleId{ get; set; }
        public RoleDetails ? RoleDetails { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
       
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
       
        public int UpdatedBy { get; set;}

        public DateTime UpdatedDate { get; set;}
        public bool StatusFlag { get; set; }  
        



    }
}
