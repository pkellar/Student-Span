using System;
using System.ComponentModel.DataAnnotations;

namespace StudentSpan.Models
{
    public class StudentSearchViewModel
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public string GradMonth { get; set; }
        public int? GradYearFrom { get; set; }
        public int? GradYearTo { get; set; }
        public string School { get; set; }
    }
}
