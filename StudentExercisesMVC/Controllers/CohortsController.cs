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
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
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

        // GET: Cohorts
        public ActionResult Index()
        {
            var cohorts = CohortRepository.GetCohorts();
            return View(cohorts);
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            var cohort = CohortRepository.GetCohort(id);
            return View(cohort);
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Cohort cohort)
        {
            Cohort newCohort = CohortRepository.CreateCohort(cohort);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            var cohort = CohortRepository.GetCohort(id);
            return View(cohort);
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] Cohort cohort)
        {
            try
            {
                cohort.Id = id;
                CohortRepository.UpdateCohort(cohort);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(cohort);
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            var cohort = CohortRepository.GetCohort(id);
            return View(cohort);
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (CohortRepository.DeleteCohort(id))
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