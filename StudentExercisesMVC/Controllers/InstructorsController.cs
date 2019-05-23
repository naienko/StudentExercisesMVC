using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;
using StudentExercisesMVC.Repositories;

namespace StudentExercisesMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public string ConnectionString
        {
            get
            {
                return _config.GetConnectionString("DefaultConnection");
            }
        }

        // GET: Instructors
        public ActionResult Index()
        {
            var instructors = InstructorRepository.GetInstructors();
            return View(instructors);
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int id)
        {
            var instructor = InstructorRepository.GetInstructor(id);
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            InstructorCreateViewModel model = new InstructorCreateViewModel(Connection);
            return View(model);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] InstructorCreateViewModel model)
        {
            var instructor = InstructorRepository.CreateInstructor(model.instructor);
            return RedirectToAction(nameof(Index));
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            InstructorEditViewModel model = new InstructorEditViewModel(id);
            return View(model);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] InstructorEditViewModel model)
        {
            try
            {
                model.instructor.Id = id;
                InstructorRepository.UpdateInstructor(model.instructor);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            var instructor = InstructorRepository.GetInstructor(id);
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (InstructorRepository.DeleteInstructor(id))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }
    }
}