using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Repositories;

namespace StudentExercisesMVC.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
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
        // GET: Exercises
        public ActionResult Index()
        {
            List<Exercise> exercises = ExerciseRepository.GetExercises();
            return View(exercises);
        }

        // GET: Exercises/Details/5
        public ActionResult Details(int id)
        {
            Exercise exercise = ExerciseRepository.GetExercise(id);
            return View(exercise);
        }

        // GET: Exercises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Exercise exercise)
        {
            var newExercise = ExerciseRepository.CreateExercise(exercise);
            return RedirectToAction(nameof(Index));
        }

        // GET: Exercises/Edit/5
        public ActionResult Edit(int id)
        {
            Exercise exercise = ExerciseRepository.GetExercise(id);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] Exercise exercise)
        {
            try
            {
                exercise.Id = id;
                ExerciseRepository.UpdateExercise(exercise);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(exercise);
            }

        }

        // GET: Exercises/Delete/5
        public ActionResult Delete(int id)
        {
            Exercise exercise = ExerciseRepository.GetExercise(id);
            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (ExerciseRepository.DeleteExercise(id))
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