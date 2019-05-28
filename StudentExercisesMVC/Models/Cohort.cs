using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Cohort
    {
		public int Id { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 5)]
        [Display(Name ="Cohort Designation")]
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

    }
}
