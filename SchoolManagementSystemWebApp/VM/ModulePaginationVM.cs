using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class ModulePaginationVM
    {
        public IEnumerable<ModuleDTO> Module { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }

    }
}
