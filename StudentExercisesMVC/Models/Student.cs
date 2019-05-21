using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string _firstname { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string _lastname { get; set; }
        [Required]
        [Display(Name = "Slack Handle")]
        [StringLength(12, MinimumLength = 3)]
        public string _handle { get; set; }
        public int _cohortId { get; set; }
        public Cohort _cohort;

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
