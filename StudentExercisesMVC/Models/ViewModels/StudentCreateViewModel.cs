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
    public class StudentCreateViewModel
    {
        //a new student
        public Student student { get; set; } = new Student();

        //all the cohorts
        public List<SelectListItem> Cohorts;

        public StudentCreateViewModel()
        {
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