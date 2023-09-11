using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class UserDTO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [ForeignKey("Register")]
        public int registerId { get; set; }
        public RegistrationDTO? Register { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [ForeignKey("RoleDetails")]
        public int RoleId { get; set; }
        public RoleDetailsDTO? RoleDetails { get; set; }
        public bool StatusFlag { get; set; }
    }
}
