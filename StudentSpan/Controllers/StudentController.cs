using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using StudentSpan.Models;
using StudentSpan.ViewModels;
using StudentSpan.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace StudentSpan.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _repo;

        public StudentController(UserManager<ApplicationUser> userManager, IStudentRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public async Task<IActionResult> Index(StudentSearchViewModel searchModel, string sortOrder)
        {
            StudentViewModel ViewModelSorting = new StudentViewModel(sortOrder);

            var students = await _repo.GetStudents();

            if ( searchModel != null )
            {
                var students1 = _repo.Filter(searchModel, students);
                var students2 = _repo.Sort(ViewModelSorting.EnumSort, students1);
                ViewModelSorting.Students = students2;
               return View(ViewModelSorting);
            }
            var students3 = _repo.Sort(ViewModelSorting.EnumSort, students);
            ViewModelSorting.Students = students3;

            return View(ViewModelSorting);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            StudentViewModel studentViewModel = new StudentViewModel(await _repo.GetStudentByID((int)id));

            return View(studentViewModel);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            return View(studentViewModel);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,GradMonth,GradYear,Degree,School")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _repo.InsertStudent(studentViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentViewModel studentViewModel = new StudentViewModel(await _repo.GetStudentByID((int)id));

            return View(studentViewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,GradMonth,GradYear,Degree,School")] StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedStudent = await _repo.UpdateStudent(studentViewModel);
                if (updatedStudent == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _repo.GetStudentByID((int)id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteStudent(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/CreateComment
        public async Task<IActionResult> CreateComment(int Id)
        {
            StudentViewModel studentViewModel = new StudentViewModel(await _repo.GetStudentByID(Id));

            return View(studentViewModel);
        }

        // POST: Student/CreateComment
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,FirstName,LastName,GradMonth,GradYear,Degree,School,CommentText,WatchText")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                studentViewModel.CommentEnteredOn = DateTime.Now;
                studentViewModel.CommentEnteredBy = await _userManager.GetUserAsync(User);
                await _repo.InsertComment(studentViewModel);
                return RedirectToAction(nameof(Details), new { studentViewModel.Id });
            }
            return View();
        }

        // GET: Student/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int id, int studentId)
        {
            var Id = studentId;

            await _repo.DeleteComment(id);
            return RedirectToAction(nameof(Details), new { Id });
        }

        // GET: Student/CreateComment
        public async Task<IActionResult> CreateWatch(int Id)
        {
            StudentViewModel studentViewModel = new StudentViewModel(await _repo.GetStudentWatchByID(Id));

            return View(studentViewModel);
        }
        // POST: Student/CreateWatch
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWatch([Bind("Id,FirstName,LastName,GradMonth,GradYear,Degree,School,CommentText,WatchText")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                studentViewModel.WatchTime = DateTime.Now;
                studentViewModel.WatchedBy = await _userManager.GetUserAsync(User);
                await _repo.InsertWatch(studentViewModel);
                return RedirectToAction(nameof(WatchPage), new { studentViewModel.Id });
            }
            return View();
        }

        // GET: Student/DeleteWatch/5
        public async Task<IActionResult> DeleteWatch(int id, int studentId)
        {
            var Id = studentId;

            await _repo.DeleteWatch(id);
            return RedirectToAction(nameof(WatchPage), new { Id });
        }

        // GET: Students/WatchPage/5
        public async Task<IActionResult> WatchPage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            StudentViewModel studentViewModel = new StudentViewModel(await _repo.GetStudentWatchByID((int)id));

            return View(studentViewModel);
        }
    }   

}
