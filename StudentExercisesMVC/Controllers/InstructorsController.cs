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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName,
                i.SlackName, i.CohortId, i.Specialty
                                FROM Instructor i";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructor instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty"))
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return View(instructors);
                }
            }
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName,
                i.SlackName, i.CohortId, i.Specialty 
                                FROM Instructor i
                                WHERE i.Id = @InstructorId";
                    cmd.Parameters.Add(new SqlParameter("@InstructorId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructor instructor = null;
                    if (reader.Read())
                    {
                        instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Specialty = reader.GetString(reader.GetOrdinal("Specialty"))
                        };
                    }

                    reader.Close();

                    return View(instructor);
                }
            }
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Instructor
                ( FirstName, LastName, SlackName, CohortId, Specialty )
                VALUES
                ( @firstName, @lastName, @slackName, @cohortId, @Specialty )";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackName", model.instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@specialty", model.instructor.Specialty));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            InstructorEditViewModel model = new InstructorEditViewModel(ConnectionString, id);
            return View(model);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] InstructorEditViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Instructor
                                SET FirstName = @firstName, LastName = @lastName, SlackName = @slackName,
                                    CohortId = @cohortId, Specialty = @specialty
                                WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackName", model.instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@specialty", model.instructor.Specialty));
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Instructor WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}