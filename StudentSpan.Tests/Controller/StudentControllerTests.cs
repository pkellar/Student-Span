using StudentSpan.Repository;
using StudentSpan.Repository.Interfaces;
using StudentSpan.Tests.Fixture;
using StudentSpan.Tests.Repository;
using StudentSpan.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StudentSpan.Controllers;
using StudentSpan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace StudentSpan.Tests.Controller
{
    public class StudentControllerTests : IClassFixture<DatabaseFixture>
    {
        private static string recruiter1 = "recruiter@example.com";
        private DatabaseFixture _fixture;
        private IStudentRepository _repo;

        private Mock<UserManager<ApplicationUser>> _mockUserManager;

        public StudentControllerTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repo = new StudentRepository(_fixture.CreateContext());

            _mockUserManager = GetUserManagerMock<ApplicationUser>();
        }

        [Fact]
        public async void Edit_ReturnsAViewResult_AsStudent()
        {
            // Arrange.
            var controller = new StudentController(_mockUserManager.Object, _repo);
            // Act.
            var actionResult = await controller.Edit(1);

            // Assert.
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<StudentViewModel>(viewResult.Model);
        }

        [Fact]
        public async void Delete_ReturnsAViewResult_AsStudent()
        {
            // Arrange.
            var controller = new StudentController(_mockUserManager.Object, _repo);
            // Act.
            var actionResult = await controller.Delete(1);

            // Assert.
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<Student>(viewResult.Model);
           
        }

        [Fact]
        public async void Details_ReturnsAViewResult_AsStudentViewModel()
        {
            // Arrange.
            var controller = new StudentController(_mockUserManager.Object, _repo);

            // Act.
            var actionResult = await controller.Details(1);

            // Assert.
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<StudentViewModel>(viewResult.Model);
        }


        [Fact]
        public async void Details_ReturnsAViewResult_AsStudentViewModel_VerifyStudent()
        {
            // Arrange.
            var controller = new StudentController(_mockUserManager.Object, _repo);

            // Act.
            var actionResult = await controller.Details(1);

            // Assert.
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            Assert.IsAssignableFrom<StudentViewModel>(viewResult.Model);

            StudentViewModel viewModel = viewResult.Model as StudentViewModel;
           

            Assert.Equal(1, viewModel.Id);
            Assert.Equal("James", viewModel.FirstName);
            Assert.Equal("Hill", viewModel.LastName);
            Assert.Equal("May", viewModel.GradMonth);
            Assert.Equal(2023, viewModel.GradYear);
            Assert.Equal("MET", viewModel.Degree);
            Assert.Equal("MIT", viewModel.School);

        }

        private static Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser
        {

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    UserName = recruiter1,
                    NormalizedUserName = recruiter1.ToUpper(),
                    Email = recruiter1,
                    NormalizedEmail = recruiter1.ToUpper(),
                    EmailConfirmed = true
                }
            }.AsQueryable();

            var mockUserManager = new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);

            mockUserManager.Setup(u => u.Users).Returns((IQueryable<TIDentityUser>)users);

            return mockUserManager;
        }
    }

}
