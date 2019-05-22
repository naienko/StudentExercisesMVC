using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class CohortEditViewModel
    {
        public Cohort cohort { get; set; } = new Cohort();

        private string _connectionString;
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public CohortEditViewModel() { }
        public CohortEditViewModel(string connectionString, int id)
        {
            _connectionString = connectionString;
            GetCohortById(id);
        }

        public void GetCohortById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Designation
                                FROM Cohort c
                                WHERE c.Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cohort.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        cohort.Name = reader.GetString(reader.GetOrdinal("Designation"));
                    }

                    reader.Close();
                }
            }
        }
    }
}
