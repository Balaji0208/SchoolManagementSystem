namespace SchoolManagementSystemWebApp.Models.DTO
{
    public class CategoriesDTO
    {

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
