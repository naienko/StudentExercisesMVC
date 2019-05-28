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
    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
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

        // GET: Students
        public ActionResult Index()
        {
            List<Student> students = StudentRepository.GetStudents();
            return View(students);
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            Student student = StudentRepository.GetStudent(id);
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            StudentCreateViewModel model = new StudentCreateViewModel();
            return View(model);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] StudentCreateViewModel model)
        {
            var student = StudentRepository.CreateStudent(model.student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            StudentEditViewModel model = new StudentEditViewModel(id);
            return View(model);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentEditViewModel model)
        {
            try
            {
                model.student.Id = id;
                StudentRepository.UpdateStudent(model.student);

                //clear out assigned exercises
                ExerciseRepository.ClearAssignedExercises(model.student.Id);

                //assign exercises selected in the form
                if (model.SelectedExercises.Count > 0)
                {
                    model.SelectedExercises.ForEach(i =>
                        ExerciseRepository.AssignToStudent(i, model.student.Id));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(model);
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = StudentRepository.GetStudent(id);
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (StudentRepository.DeleteStudent(id))
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