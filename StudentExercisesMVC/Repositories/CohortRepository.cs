using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Repositories
{
    public class CohortRepository
    {
        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        //getall
        public static List<Cohort> GetCohorts()
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
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }

        //getone
        public static Cohort GetCohort(int id)
        {
            Cohort cohort = null;
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

                    if (reader.Read())
                    {
                        cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Designation")),
                        };
                    }

                    reader.Close();

                    return cohort;
                }
            }
        }

        //create
        public static Cohort CreateCohort(Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort ( Designation )
                                OUTPUT INSERTED.Id
                                VALUES ( @designation )";
                    cmd.Parameters.Add(new SqlParameter("@Designation", cohort.Name));

                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return cohort;
                }
            }
        }
        //edit
        public static void UpdateCohort(Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Cohort
                                SET Designation = @designation
                                WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Designation", cohort.Name));
                    cmd.Parameters.Add(new SqlParameter("@Id", cohort.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //delete
        public static bool DeleteCohort(int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0) return false;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
