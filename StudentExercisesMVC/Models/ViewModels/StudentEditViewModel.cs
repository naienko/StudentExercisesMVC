using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentEditViewModel
    {
        //a student
        public Student student { get; set; }

        //all cohorts
        public List<SelectListItem> Cohorts;

        [Display(Name = "Assigned Exercises")]
        public List<SelectListItem> Exercises { get; private set; }

        public List<int> SelectedExercises { get; set; }

        public StudentEditViewModel() { }
        public StudentEditViewModel(int id)
        {
            student = StudentRepository.GetStudent(id);
            CohortSelectFactory();
            ExerciseSelectFactory();
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

        private void ExerciseSelectFactory()
        {
            //var exercises = ExerciseRepository.GetExercises();
            Exercises = ExerciseRepository.GetExercises()
                .Select(e => new SelectListItem
                {
                    Text = e.Title,
                    Value = e.Id.ToString(),
                    Selected = student.AssignedExercises.Find(ex => ex.Id == e.Id) != null
                }).ToList();
        }
    }
}
