using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class RegistrationViewModel
    {

        public RegistrationViewModel()
        {
            Registration = new RegistrationDTO();
        }
        public RegistrationDTO Registration { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
