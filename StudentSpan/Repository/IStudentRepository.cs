using System;
using System.Collections.Generic;
using StudentSpan.Models;
using System.Threading.Tasks;
using StudentSpan.ViewModels;

namespace StudentSpan.Repository.Interfaces
{
    public interface IStudentRepository : IDisposable
    {
        enum sorting
        {
            FIRST = 0,
            FIRSTDESC = 1,
            LAST = 2,
            LASTDESC = 3,
            MONTH = 4,
            MONTHDESC = 5,
            YEAR = 6,
            YEARDESC = 7,
            DEGREE = 8,
            DEGREEDESC = 9,
            SCHOOL = 10,
            SCHOOLDESC = 11
        }
        Task<IEnumerable<Student>> GetStudents();
        Task<IList<Student>> GetStudents(string searchString);
        Task<IEnumerable<Student>> GetStudentsWatch();
        Task<IList<Student>> GetStudentsWatch(string searchString);
        Task<Student> GetStudentByID(int studentId);
        Task<Student> GetStudentWatchByID(int studentId);
        Task<Student> InsertStudent(StudentViewModel studentViewModel);
        Task<Student> DeleteStudent(int studentID);
        Task<Student> UpdateStudent(StudentViewModel studentViewModel);
        IEnumerable<Student> Filter(StudentSearchViewModel searchModel, IEnumerable<Student> users);
        IEnumerable<Student> Sort(sorting sortOrder, IEnumerable<Student> users);
        Task<IList<Comment>> GetComments();
        Task<Comment> InsertComment(StudentViewModel studentViewModel);
        Task DeleteComment(int commentID);

        Task<IList<Watch>> GetWatches();
        Task<Watch> InsertWatch(StudentViewModel studentViewModel);
        Task DeleteWatch(int WatchID);


    }
}