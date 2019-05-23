using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
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

        public StudentEditViewModel() { }
        public StudentEditViewModel(int id)
        {
            CohortSelectFactory();
            student = StudentRepository.GetStudent(id);
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
