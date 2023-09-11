using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class RoleVM
    {
        public IEnumerable<RoleDetailsDTO> Role { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }

    }
}
