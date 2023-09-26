namespace SchoolManagementSystemWebApp.Models
{
    public class CustomSelectedItem
    {

        public string Text { get; set; }    
        public string Value { get; set; }
        public bool Selected { get; set; }  
        public int? ParentId { get; set; }
        public string ParentName { get; set; }  
       
    }
}
