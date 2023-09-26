using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class ModulesVM
    {
        public ModulesVM()
        {
            modulesVM = new ModuleDTO();
        }
        public ModuleDTO modulesVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ParentList { get; set; }
    }
}
