using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class RegistrationDTO
    {
        [Required]
        public string FirstName { get; set; }
        public int categoryId { get; set; }

       public CategoriesDTO Categories{ get; set; }
        
        public int RegId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
       
        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }
    }
}
