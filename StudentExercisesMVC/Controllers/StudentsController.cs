﻿using System;
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName,
                s.SlackName, s.CohortId
                                FROM Student s";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return View(students);
                }
            }
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName,
                s.SlackName, s.CohortId
                                FROM Student s
                                WHERE s.Id = @StudentId";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };
                    }

                    reader.Close();

                    return View(student);
                }
            }
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            StudentCreateViewModel model = new StudentCreateViewModel(Connection);
            return View(model);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] StudentCreateViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student
                ( FirstName, LastName, SlackName, CohortId )
                VALUES
                ( @firstName, @lastName, @slackName, @cohortId )";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackName", model.student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.student.CohortId));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            StudentEditViewModel model = new StudentEditViewModel(ConnectionString, id);
            return View(model);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] StudentEditViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Student
                                SET FirstName = @firstName, LastName = @lastName, SlackName = @slackName,
                                    CohortId = @cohortId
                                WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackName", model.student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.student.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}