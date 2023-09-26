using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolManagementSystemWebApp.Models;
using SchoolManagementSystemWebApp.Models.DTO;
using System.Data;

namespace SchoolManagementSystemWebApp.VM
{
    public class ModuleMappinVM
    {

        public ModuleMappinVM()
        {
            ModuleMapping = new ModuleRoleMappingDTO();
             
        }
        
        public ModuleRoleMappingDTO ModuleMapping { get; set; }
        
       
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
        [ValidateNever]
        public IEnumerable<CustomSelectedItem> MenuList { get; set; }
        [ValidateNever]
    
        public IEnumerable<ModuleRoleMappingDTO> MainMenus { get; set; }





    }
}
