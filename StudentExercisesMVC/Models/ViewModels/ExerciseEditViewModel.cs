using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;


namespace StudentExercisesMVC.Models.ViewModels
{
    public class ExerciseEditViewModel
    {
        public Exercise exercise { get; set; } = new Exercise();

        private string _connectionString;
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public ExerciseEditViewModel() { }
        public ExerciseEditViewModel(string connectionString, int id)
        {
            _connectionString = connectionString;
            GetExerciseById(id);
        }

        public void GetExerciseById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Title, e.Language, e.Id
                                FROM Exercise e
                                WHERE e.Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        exercise.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        exercise.Title = reader.GetString(reader.GetOrdinal("Title"));
                        exercise.Language = reader.GetString(reader.GetOrdinal("Language"));
                    }

                    reader.Close();
                }
            }
        }
    }
}
