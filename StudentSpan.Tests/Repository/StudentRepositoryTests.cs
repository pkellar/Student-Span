using StudentSpan.Models;
using StudentSpan.Repository;
using StudentSpan.Repository.Interfaces;
using StudentSpan.Tests.Fixture;
using StudentSpan.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace StudentSpan.Tests.Repository
{
    public class StudentRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture _fixture;
        private IStudentRepository _repo;


        public StudentRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repo = new StudentRepository(_fixture.CreateContext());
        }

        [Fact]
        public async void Get_Students()
        {
            IList<Student> students = await _repo.GetStudents(string.Empty);

            Assert.Equal(4, students.Count);

            Assert.Collection(students,
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Timmy", s.LastName),
                s => Assert.Equal("Blaze", s.LastName),
                s => Assert.Equal("Hill", s.LastName));
        }

        [Fact]
        public async void Get_Students_BySearch_None()
        {
            // Arrange.
            var searchString = "qqqxxx";

            // Act.
            IList<Student> students = await _repo.GetStudents(searchString);

            // Assert.
            Assert.Equal(0, students.Count);
        }

        [Fact]
        public async void Get_Students_BySearch_One()
        {
            var searchString = "Timmy";

            IList<Student> students = await _repo.GetStudents(searchString);

            Assert.Equal(1, students.Count);

            Assert.Collection(students,
                s => Assert.Equal("Timmy", s.LastName));
        }

        [Fact]
        public async void Get_Students_BySearch_Many()
        {
            var searchString = "Hill";

            IList<Student> students = await _repo.GetStudents(searchString);

            Assert.Equal(2, students.Count);

            Assert.Collection(students,
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Hill", s.LastName));
        }

        [Fact]
        public async void Insert_Student()
        {
            var viewModel = new StudentViewModel
            {
                FirstName = "David",
                LastName = "Jones",
                GradMonth = "June",
                GradYear = 2023,
                Degree = "CSC",
                School = "Texas Tech"
            };

            Student newStudent = await _repo.InsertStudent(viewModel);
            Student student = await _repo.GetStudentByID(newStudent.Id);


            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);
            await _repo.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async void Update_Student()
        {
            // Arrange
            string tempLastName = "Barbra";

            var viewModel = new StudentViewModel
            {
                FirstName = "Susan",
                LastName = "Robberts",
                GradMonth = "October",
                GradYear = 2024,
                Degree = "MET",
                School = "Kansas"
            };

            Student newStudent = await _repo.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            viewModel.FirstName = newStudent.FirstName;
            viewModel.LastName = tempLastName;

            Student updateStudent = await _repo.UpdateStudent(viewModel);

            Assert.IsAssignableFrom<Student>(updateStudent);
            Assert.Equal(updateStudent.LastName, tempLastName);
            await _repo.DeleteStudent(viewModel.Id);

        }

        [Fact]
        public async void Delete_Student()
        {
            var viewModel = new StudentViewModel
            {
                FirstName = "Jimmy",
                LastName = "Neutron",
                GradMonth = "June",
                GradYear = 2025,
                Degree = "CENG",
                School = "Texas"
            };

            Student newStudent = await _repo.InsertStudent(viewModel);

            int id = newStudent.Id;
            await _repo.DeleteStudent(id);

            Student deleteStudent = await _repo.GetStudentByID(id);

            Assert.Null(deleteStudent);
        }

        [Fact]
        public async void Get_Comments()
        {
            // Arrange.

            // Act.
            IList<Comment> comments = await _repo.GetComments();

            // Assert.
            Assert.Equal(0, comments.Count);
        }

        [Fact]
        public async void Get_Student_ById()
        {

            var viewModel = new StudentViewModel
            {
                FirstName = "Jimmy",
                LastName = "Neutron",
                GradMonth = "June",
                GradYear = 2025,
                Degree = "CENG",
                School = "Texas"
            };

            Student newStudent = await _repo.InsertStudent(viewModel);

            Student getStudent = await _repo.GetStudentByID(newStudent.Id);


            Assert.Same(newStudent, getStudent);
            Assert.Equal(getStudent.LastName, viewModel.LastName);
            await _repo.DeleteStudent(newStudent.Id);

        }

        // filter and sort tests
        [Fact]
        public async void Filter_Students_Name()
        {   
            var searchModel1 = new StudentSearchViewModel();
            var searchModel2 = new StudentSearchViewModel();

            searchModel1.FirstName = "James";
            searchModel2.LastName = "Hill";

            IList<Student> students = await _repo.GetStudents(string.Empty);

            // test both name filters

            IEnumerable<Student> students2 = _repo.Filter(searchModel1, students);

            Assert.Equal(1, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("James", s.FirstName));

            students2 = _repo.Filter(searchModel2, students);

            Assert.Equal(2, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Hill", s.LastName));
        }

        [Fact]
        public async void Filter_Students_Month()
        {
            //model for month 
            var searchModel = new StudentSearchViewModel();

            searchModel.GradMonth = "May";

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Filter(searchModel, students);

            students2 = _repo.Filter(searchModel, students);

            Assert.Equal(1, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("May", s.GradMonth));

        }

        [Fact]
        public async void Filter_Students_Year()
        {
            var searchModel = new StudentSearchViewModel();

            searchModel.GradYearFrom = 2023;
            searchModel.GradYearTo = 2023;
            
            IList<Student> students = await _repo.GetStudents(string.Empty);

            //test year filters
            IEnumerable<Student> students2 = _repo.Filter(searchModel, students);

            Assert.Equal(2, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal(2023, s.GradYear),
                s => Assert.Equal(2023, s.GradYear));

        }

        [Fact]
        public async void Filter_Students_Degree()
        {
            //degree model
            var searchModel = new StudentSearchViewModel();

            searchModel.Degree = "MET";

            IList<Student> students = await _repo.GetStudents(string.Empty);

            //test degree filter
            IEnumerable<Student> students2 = _repo.Filter(searchModel, students);

            Assert.Equal(1, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("MET", s.Degree));
        }

        [Fact]
        public async void Filter_Students_School()
        {
            //school model
            var searchModel = new StudentSearchViewModel();

            searchModel.School = "MIT";

            IList<Student> students = await _repo.GetStudents(string.Empty);

            // test school filter

            IEnumerable<Student> students2 = _repo.Filter(searchModel, students);
            Assert.Equal(2, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("MIT", s.School),
                s => Assert.Equal("MIT", s.School));

        }

        [Fact]
        public async void Sort_Students_FirstName()
        {
            IStudentRepository.sorting sortOrder1 = 0;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)1;

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Allen", s.FirstName),
                s => Assert.Equal("James", s.FirstName),
                s => Assert.Equal("Jane", s.FirstName),
                s => Assert.Equal("Paul", s.FirstName));

            students2 = _repo.Sort(sortOrder2, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Paul", s.FirstName),
                s => Assert.Equal("Jane", s.FirstName),
                s => Assert.Equal("James", s.FirstName),
                s => Assert.Equal("Allen", s.FirstName));
        }

        [Fact]
        public async void Sort_Students_LastName()
        {
            IStudentRepository.sorting sortOrder1 = (IStudentRepository.sorting)2;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)3;

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Blaze", s.LastName),
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Timmy", s.LastName));

            students2 = _repo.Sort(sortOrder2, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Timmy", s.LastName),
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Hill", s.LastName),
                s => Assert.Equal("Blaze", s.LastName));
        }

        [Fact]
        public async void Sort_Students_Month()
        {
            IStudentRepository.sorting sortOrder1 = (IStudentRepository.sorting)4;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)5;

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("July", s.GradMonth),
                s => Assert.Equal("July", s.GradMonth),
                s => Assert.Equal("June", s.GradMonth),
                s => Assert.Equal("May", s.GradMonth));

            students2 = _repo.Sort(sortOrder2, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("May", s.GradMonth),
                s => Assert.Equal("June", s.GradMonth),
                s => Assert.Equal("July", s.GradMonth),
                s => Assert.Equal("July", s.GradMonth));
        }

        [Fact]
        public async void Sort_Students_Year()
        {
            IStudentRepository.sorting sortOrder1 = (IStudentRepository.sorting)6;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)7;

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students);

            Assert.Equal(4, (int)students2.Count());
            Assert.Collection(students2,
                s => Assert.Equal(2023, s.GradYear),
                s => Assert.Equal(2023, s.GradYear),
                s => Assert.Equal(2024, s.GradYear),
                s => Assert.Equal(2025, s.GradYear));

            students2 = _repo.Sort(sortOrder2, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal(2025, s.GradYear),
                s => Assert.Equal(2024, s.GradYear),
                s => Assert.Equal(2023, s.GradYear),
                s => Assert.Equal(2023, s.GradYear));
        }

        [Fact]
        public async void Sort_Students_Degree()
        {
            IStudentRepository.sorting sortOrder1 = (IStudentRepository.sorting)8;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)9;

            IList<Student> students1 = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students1);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("CENG", s.Degree),
                s => Assert.Equal("CSC", s.Degree),
                s => Assert.Equal("CSC", s.Degree),
                s => Assert.Equal("MET", s.Degree));

            students2 = _repo.Sort(sortOrder2, students1);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("MET", s.Degree),
                s => Assert.Equal("CSC", s.Degree),
                s => Assert.Equal("CSC", s.Degree),
                s => Assert.Equal("CENG", s.Degree));
        }

        [Fact]
        public async void Sort_Students_School()
        {
            IStudentRepository.sorting sortOrder1 = (IStudentRepository.sorting)10;
            IStudentRepository.sorting sortOrder2 = (IStudentRepository.sorting)11;

            IList<Student> students = await _repo.GetStudents(string.Empty);

            IEnumerable<Student> students2 = _repo.Sort(sortOrder1, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("Harbard", s.School),
                s => Assert.Equal("MIT", s.School),
                s => Assert.Equal("MIT", s.School),
                s => Assert.Equal("USD", s.School));

            students2 = _repo.Sort(sortOrder2, students);

            Assert.Equal(4, (int)students2.Count());

            Assert.Collection(students2,
                s => Assert.Equal("USD", s.School),
                s => Assert.Equal("MIT", s.School),
                s => Assert.Equal("MIT", s.School),
                s => Assert.Equal("Harbard", s.School));
        }
    }
}
