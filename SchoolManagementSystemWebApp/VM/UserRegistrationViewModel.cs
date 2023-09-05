using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class UserRegistrationViewModel
    {

        public UserRegistrationViewModel()
        {
            UserRegistration = new UserDTO();
        }
        public UserDTO UserRegistration { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RegisterList { get; set; }
    
}
}
