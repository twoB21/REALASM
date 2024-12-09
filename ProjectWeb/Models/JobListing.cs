using Microsoft.AspNetCore.Identity;

namespace ProjectWeb.Models
{
    public class JobListing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequiredQualifications { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string EmployerId { get; set; }  
        public IdentityUser Employer { get; set; }
    }
}
