using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Language { get; set; }
        public List<Student> assignedStudents { get; set; }
    }
}
