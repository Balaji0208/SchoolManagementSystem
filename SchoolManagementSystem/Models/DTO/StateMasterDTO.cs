using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models.DTO
{
    public class StateMasterDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        [ForeignKey("CountryMaster")]
        public int CountryId { get; set; }
        public CountryMaster? CountryMaster { get; set; }

    }
}
