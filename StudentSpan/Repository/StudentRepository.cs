using StudentSpan.Data;
using StudentSpan.Models;
using StudentSpan.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentSpan.ViewModels;

namespace StudentSpan.Repository
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private StudentSpanContext _context;

        public StudentRepository(StudentSpanContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<Student> DeleteStudent(int studentID)
        {
            var student = await _context.StudentModel.FindAsync(studentID);
            _context.StudentModel.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> GetStudentByID(int studentId)
        {
            return await _context.StudentModel.Include(c => c.Comments)
                .ThenInclude(u => u.ApplicationUser).FirstOrDefaultAsync(s => s.Id == studentId);
            //return await _context.StudentModel.Include(w => w.Watches)
            //    .ThenInclude(u => u.ApplicationUser).FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<Student> GetStudentWatchByID(int studentId)
        {

            return await _context.StudentModel.Include(w => w.Watches)
                .ThenInclude(u => u.ApplicationUser).FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<Student> GetStudentByID(Guid guid)
        {
            return await _context.StudentModel.FindAsync(guid);
        }

        public async Task<Student> GetStudentWatchByID(Guid guid)
        {
            return await _context.StudentModel.FindAsync(guid);
        }


        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.StudentModel.ToListAsync();
        }

        public async Task<IList<Student>> GetStudents(string searchString)
        {
            var students = from s in _context.StudentModel select s;

            if(!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s =>
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString)
                    );
            }

            return await students.Include(c => c.Comments).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWatch()
        {
            return await _context.StudentModel.ToListAsync();
        }

        public async Task<IList<Student>> GetStudentsWatch(string searchString)
        {
            var students = from s in _context.StudentModel select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s =>
                    s.FirstName.Contains(searchString) ||
                    s.LastName.Contains(searchString)
                    );
            }

            return await students.Include(w => w.Watches).ToListAsync();
        }

        public async Task<Student> InsertStudent(StudentViewModel studentViewModel)
        {
            Student student = new()
            {
                FirstName = studentViewModel.FirstName.Trim(),
                LastName = studentViewModel.LastName.Trim(),
                GradMonth = studentViewModel.GradMonth.Trim(),
                GradYear = studentViewModel.GradYear,
                Degree = studentViewModel.Degree.Trim(),
                School = studentViewModel.School.Trim(),

            };
            _context.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<Student> UpdateStudent(StudentViewModel studentViewModel)
        {
            Student student;
            try
            {
                student = await _context.StudentModel.FindAsync(studentViewModel.Id);
                student.FirstName = studentViewModel.FirstName.Trim();
                student.LastName = studentViewModel.LastName.Trim();
                student.GradMonth = studentViewModel.GradMonth.Trim();
                student.GradYear = studentViewModel.GradYear;
                student.Degree = studentViewModel.Degree.Trim();
                student.School = studentViewModel.School.Trim();

                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(studentViewModel.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return student;
        }
       
        public IEnumerable<Student> Filter(StudentSearchViewModel searchModel, IEnumerable<Student> users)
        {
            var students = users;
            if (searchModel.Id.HasValue)
                students = students.Where(x => x.Id == searchModel.Id);
            if (!string.IsNullOrEmpty(searchModel.FirstName))
                students = students.Where(x => x.FirstName.Contains(searchModel.FirstName));
            if (!string.IsNullOrEmpty(searchModel.LastName))
                students = students.Where(x => x.LastName.Contains(searchModel.LastName));
            if (!string.IsNullOrEmpty(searchModel.Degree))
                students = students.Where(x => x.Degree.Contains(searchModel.Degree));
            if (!string.IsNullOrEmpty(searchModel.GradMonth))
                students = students.Where(x => x.GradMonth.Contains(searchModel.GradMonth));
            if (searchModel.GradYearFrom.HasValue)
                students = students.Where(x => x.GradYear >= searchModel.GradYearFrom);
            if (searchModel.GradYearTo.HasValue)
                students = students.Where(x => x.GradYear <= searchModel.GradYearTo);
            if (!string.IsNullOrEmpty(searchModel.School))
                students = students.Where(x => x.School.Contains(searchModel.School));

            return students;
        }

        public IEnumerable<Student> Sort(IStudentRepository.sorting sortOrder, IEnumerable<Student> users)
        {
            var students = users;
            
            switch (sortOrder)
            {
                
                case IStudentRepository.sorting.FIRST:
                    students = students.OrderBy(s => s.FirstName);
                    break;
                case IStudentRepository.sorting.FIRSTDESC:
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case IStudentRepository.sorting.LASTDESC:
                    students = students.OrderByDescending(s => s.LastName);
                    break;                   
                case IStudentRepository.sorting.MONTH:
                    students = students.OrderBy(s => s.GradMonth);
                    break;
                case IStudentRepository.sorting.MONTHDESC:
                    students = students.OrderByDescending(s => s.GradMonth);
                    break;
                case IStudentRepository.sorting.YEAR:
                    students = students.OrderBy(s => s.GradYear);
                    break;
                case IStudentRepository.sorting.YEARDESC:
                    students = students.OrderByDescending(s => s.GradYear);
                    break;
                case IStudentRepository.sorting.DEGREE:
                    students = students.OrderBy(s => s.Degree);
                    break;
                case IStudentRepository.sorting.DEGREEDESC:
                    students = students.OrderByDescending(s => s.Degree);
                    break;
                case IStudentRepository.sorting.SCHOOL:
                    students = students.OrderBy(s => s.School);
                    break;
                case IStudentRepository.sorting.SCHOOLDESC:
                    students = students.OrderByDescending(s => s.School);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            return students;
        }

        public async Task<IList<Comment>> GetComments()
        {
            return await _context.Comment.Include(s => s.Student).Include(u => u.ApplicationUser).ToListAsync();
        }

        public async Task<Comment> InsertComment(StudentViewModel studentViewModel)
        {
            Student student = await _context.StudentModel.FindAsync(studentViewModel.Id);

            Comment comment = new()
            {
                Student = student,
                ApplicationUser = studentViewModel.CommentEnteredBy,
                EnteredOn = studentViewModel.CommentEnteredOn,
                Text = studentViewModel.CommentText
            };

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteComment(int commentID)
        {
            Comment comment = await _context.Comment.FindAsync(commentID);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Watch>> GetWatches()
        {
            return await _context.Watch.Include(s => s.Student).Include(u => u.ApplicationUser).ToListAsync();
        }

        public async Task<Watch> InsertWatch(StudentViewModel studentViewModel)
        {
            Student student = await _context.StudentModel.FindAsync(studentViewModel.Id);

            Watch watch = new()
            {
                Student = student,
                ApplicationUser = studentViewModel.WatchedBy,
                WatchedOn = studentViewModel.WatchTime,
                Text = studentViewModel.WatchText
            };

            _context.Watch.Add(watch);
            await _context.SaveChangesAsync();

            return watch;
        }

        public async Task DeleteWatch(int watchID)
        {
            Watch watch = await _context.Watch.FindAsync(watchID);
            _context.Watch.Remove(watch);
            await _context.SaveChangesAsync();
        }

        private bool StudentExists(int id)
        {
            return _context.StudentModel.Any(e => e.Id == id);
        }
    }
}
