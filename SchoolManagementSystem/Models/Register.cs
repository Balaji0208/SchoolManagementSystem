using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Register
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int registerId { get; set;}

        [ForeignKey("Categories")]
        public int categoryId { get;set; }
        public Categories Categories { get; set; }

        
        
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
       
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string  Gender { get; set; }
        [Required]
        [MaxLength(30)]
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public DateTime joiningDate { get; set; }
        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set;}
        public bool StatusFlag { get; set; }
        }
    }

