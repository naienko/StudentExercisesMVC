using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        public Student student { get; set; } = new Student();

        public List<SelectListItem> Cohorts;

        public SqlConnection Connection;

        public  StudentCreateViewModel() { }
        public StudentCreateViewModel(SqlConnection connection)
        {
            Connection = connection;
            GetAllCohorts();
        }

        public void GetAllCohorts ()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
            SELECT c.Id,
                c.Name
            FROM Cohort c
        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>;

                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
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
