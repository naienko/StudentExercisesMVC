using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class InstructorEditViewModel
    {
        public Instructor instructor { get; set; } = new Instructor();

        public List<SelectListItem> Cohorts;

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        
        public InstructorEditViewModel() { }
        public InstructorEditViewModel(string connectionString, int id)
        {
            _connectionString = connectionString;
            GetAllCohorts();
            GetInstructorById(id);
        }

        public void GetInstructorById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT i.Id, i.FirstName, i.LastName, i.SlackName, i.CohortId, i.Specialty
                        FROM Instructor i
                        WHERE i.Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        instructor.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        instructor.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        instructor.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        instructor.SlackHandle = reader.GetString(reader.GetOrdinal("SlackName"));
                        instructor.CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        instructor.Specialty = reader.GetString(reader.GetOrdinal("Specialty"));
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
