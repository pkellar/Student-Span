using System;
using Microsoft.AspNetCore.Identity;

namespace StudentSpan.Areas.Identity.Data
{
    public class StudentSpanUser : IdentityUser
    {
        [PersonalData]
        public string firstName { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}