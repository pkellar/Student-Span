using Microsoft.AspNetCore.Identity;

namespace StudentSpan.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string CompanyName { get; set; }

        [PersonalData]
        public string Title { get; set; }
    }
}
