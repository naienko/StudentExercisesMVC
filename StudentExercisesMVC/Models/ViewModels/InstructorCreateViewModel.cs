using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class InstructorCreateViewModel
    {
        public Instructor instructor { get; set; } = new Instructor();

        public List<SelectListItem> Cohorts;

        public SqlConnection Connection;

        public InstructorCreateViewModel() { }
        public InstructorCreateViewModel(SqlConnection connection)
        {
            Connection = connection;
            CohortSelectFactory();
        }

        public void CohortSelectFactory()
        {
            var cohorts = CohortRepository.GetCohorts();
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
    }
}
