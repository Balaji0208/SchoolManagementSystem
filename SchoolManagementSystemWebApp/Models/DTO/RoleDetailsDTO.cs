namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class RoleDetailsDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool StatusFlag { get; set; }
    }
}
