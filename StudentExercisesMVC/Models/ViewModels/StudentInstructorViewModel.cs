using StudentExercisesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises.Models.ViewModels
{
    public class StudentInstructorViewModel
    {

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public StudentInstructorViewModel(string connectionString)
        {
            _connectionString = connectionString;
            GetAllStudents();
            GetAllInstructors();
        }

        private void GetAllStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id,
                            FirstName,
                            LastName,
                            SlackName
                        FROM Student";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> Students = new List<Student>();
                    while (reader.Read())
                    {
                        Students.Add(new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            _firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            _lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            _handle = reader.GetString(reader.GetOrdinal("SlackName")),
                        });
                    }

                    reader.Close();
                }
            }
        }

        private void GetAllInstructors()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT
                            Id,
                            FirstName,
                            LastName,
                            SlackHandle
                        FROM Instructor";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Instructors = new List<Instructor>();
                    while (reader.Read())
                    {
                        Instructors.Add(new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                        });
                    }

                    reader.Close();
                }
            }
        }
    }
}