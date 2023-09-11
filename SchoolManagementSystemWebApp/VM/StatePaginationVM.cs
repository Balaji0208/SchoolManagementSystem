using SchoolManagementSystemWebApp.Models.DTO;

namespace SchoolManagementSystemWebApp.VM
{
    public class StatePaginationVM
    {
        public IEnumerable<StateMasterDTO> State { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }
    }
}
