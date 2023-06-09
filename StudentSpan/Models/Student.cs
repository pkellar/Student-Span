using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

namespace StudentSpan.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GradMonth { get; set; }

        public int GradYear { get; set; }

        public string Degree { get; set; }

        public string School { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Watch> Watches { get; set; }
    }
}
