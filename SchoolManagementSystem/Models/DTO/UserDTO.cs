using Azure.Identity;

namespace SchoolManagementSystem.Models.DTO
{
    public class UserDTO
    {
        public int RegId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; } 
    }
}
