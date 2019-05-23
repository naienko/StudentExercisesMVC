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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Designation
                                FROM Cohort c";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        Cohort student = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };

                        cohorts.Add(student);
                    }

                    reader.Close();

                    return View(cohorts);
                }
            }
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Designation
                                FROM Cohort c
                                WHERE c.Id = @CohortId";
                    cmd.Parameters.Add(new SqlParameter("@CohortId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;
                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };
                    }

                    reader.Close();

                    return View(cohort);
                }
            }
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort
                ( Designation )
                VALUES
                ( @Name )";
                    cmd.Parameters.Add(new SqlParameter("@Name", cohort.Name));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Designation
                                FROM Cohort c
                                WHERE c.Id = @CohortId";
                    cmd.Parameters.Add(new SqlParameter("@CohortId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;
                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };
                    }

                    reader.Close();

                    return View(cohort);
                }
            }
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Cohort 
                                SET Designation = @Designation
                                WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Designation", cohort.Name));
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Cohorts/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Designation
                                FROM Cohort c
                                WHERE c.Id = @CohortId";
                    cmd.Parameters.Add(new SqlParameter("@CohortId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Cohort cohort = null;
                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };
                    }

                    reader.Close();

                    return View(cohort);
                }
            }
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}