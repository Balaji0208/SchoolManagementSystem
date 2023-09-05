using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models.DTO
{
    public class RegistrationDTO
    {
        public int registerId { get; set; }
        public string EmployeeId { get; set; }

        [ForeignKey("Categories")]
        public int categoryId { get; set; }
        public Categories? Categories { get; set; }
        public string FirstName { get; set; }
     
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }

        [ForeignKey("StateMaster")]
        public int StateId { get; set; }
        public StateMaster? CountryMaster { get; set; }
        [ForeignKey("CountryMaster")]
        public int CountryId { get; set; }
        public CountryMaster? Country { get; set; }


        public DateTime joiningDate { get; set; }

    }
}
