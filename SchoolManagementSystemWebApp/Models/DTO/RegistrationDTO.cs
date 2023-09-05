using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class RegistrationDTO
    {
        public string EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public int categoryId { get; set; }

       public CategoriesDTO? Categories{ get; set; }

        public int registerId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [ForeignKey("StateMaster")]
        public int StateId { get; set; }
        public StateMasterDTO? StateMaster { get; set; }
        [ForeignKey("CountryMaster")]
        public int CountryId { get; set; }
        public CountryMasterDTO? CountryMaster { get; set; }

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
        public bool StatusFlag { get; set; }


    }
}
