using Azure.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models.DTO
{
    public class UserDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [ForeignKey("Register")]
        public int registerId { get; set; }
        public Register? Register { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }


        [ForeignKey("RoleDetails")]
        public int RoleId { get; set; }
        public RoleDetails? RoleDetails { get; set; }
    }
}
