using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 3)]
        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }
        public string Specialty { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
    }
}
