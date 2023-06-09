using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using StudentSpan.Models;
using System.ComponentModel.DataAnnotations;
using StudentSpan.Repository.Interfaces;

namespace StudentSpan.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        { //constructor

        }

        public StudentViewModel( Student student )
        {
            if (student != null)
            {
                Id = student.Id;
                FirstName = student.FirstName;
                LastName = student.LastName;
                GradMonth = student.GradMonth;
                GradYear = student.GradYear;
                Degree = student.Degree;
                School = student.School;
                Comments = student.Comments;
                Watches = student.Watches;
            }
        }

        public StudentViewModel(string sortOrder)
        {
            FirstNameSortParm = sortOrder == "first" ? "first_desc" : "first";
            LastNameSortParm = sortOrder == "last" ? "last_desc" : "last";
            DateSortParm = sortOrder == "Month" ? "month_desc" : "Month";
            YearSortParm = sortOrder == "Year" ? "year_desc" : "Year";
            DegreeSortParm = sortOrder == "Degree" ? "degree_desc" : "Degree";
            SchoolSortParm = sortOrder == "School" ? "school_desc" : "School";
            if (sortOrder == "first")
            {
                EnumSort = (IStudentRepository.sorting)0;
            }
            else if (sortOrder == "first_desc")
            {
                EnumSort = (IStudentRepository.sorting)1;
            }

            if (sortOrder == "last" || String.IsNullOrEmpty(sortOrder))
            {
                EnumSort = (IStudentRepository.sorting)2;
            }
            else if (sortOrder == "last_desc")
            {
                EnumSort = (IStudentRepository.sorting)3;
            }

            if (sortOrder == "Month")
            {
                EnumSort = (IStudentRepository.sorting)4;
            }
            else if (sortOrder == "month_desc")
            {
                EnumSort = (IStudentRepository.sorting)5;
            }

            if (sortOrder == "Year")
            {
                EnumSort = (IStudentRepository.sorting)6;
            }
            else if (sortOrder == "year_desc")
            {
                EnumSort = (IStudentRepository.sorting)7;
            }

            if (sortOrder == "Degree")
            {
                EnumSort = (IStudentRepository.sorting)8;
            }
            else if (sortOrder == "degree_desc")
            {
                EnumSort = (IStudentRepository.sorting)9;
            }

            if (sortOrder == "School")
            {
                EnumSort = (IStudentRepository.sorting)10;
            }
            else if (sortOrder == "school_desc")
            {
                EnumSort = (IStudentRepository.sorting)11;
            }
        }

        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(20, MinimumLength = 3)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(20, MinimumLength = 3)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(15, MinimumLength = 3)]
        [Display(Name = "Grad Month")]
        public string GradMonth { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [Display(Name = "Grad Year")]

        public int GradYear { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(30, MinimumLength = 2)]
        public string Degree { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(40, MinimumLength = 2)]
        public string School { get; set; }

        [DisplayName("Comment")]
        public string CommentText { get; set; }

        [DisplayName("Date")]
        public DateTime CommentEnteredOn { get; set; }

        [DisplayName("User")]
        public ApplicationUser CommentEnteredBy { get; set; }

        [DisplayName("Watch Notes")]
        public string WatchText { get; set; }

        [DisplayName("Date")]
        public DateTime WatchTime { get; set; }

        [DisplayName("Watched By")]
        public ApplicationUser WatchedBy { get; set; }

        // Helper Attributes
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Watch> Watches { get; set; }
        public string FirstNameSortParm { get; set; }
        public string LastNameSortParm { get; set; }
        public string DateSortParm { get; set; }
        public string YearSortParm { get; set; }
        public string DegreeSortParm { get; set; }
        public string SchoolSortParm { get; set; }
        public IStudentRepository.sorting EnumSort { get; set; }
    }
}
