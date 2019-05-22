using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public Student student { get; set; } = new Student();

        public List<SelectListItem> Cohorts;

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public StudentEditViewModel() { }
        public StudentEditViewModel(string connectionString, int id)
        {
            _connectionString = connectionString;
            GetAllCohorts();
            GetStudentById(id);
        }

        public void GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackName, s.CohortId
                        FROM Student s
                        WHERE s.Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        student.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        student.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        student.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        student.SlackHandle = reader.GetString(reader.GetOrdinal("SlackName"));
                        student.CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                    }

                    reader.Close();
                }
            }
        }

        public void GetAllCohorts()
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
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };

                        cohorts.Add(cohort);
                        Cohorts = cohorts.Select(li => new SelectListItem
                        {
                            Text = li.Name,
                            Value = li.Id.ToString()
                        }).ToList();

                        Cohorts.Insert(0, new SelectListItem
                        {
                            Text = "Choose cohort ...",
                            Value = "0",
                        });

                    }

                    reader.Close();
                }
            }
        }
    }
}
