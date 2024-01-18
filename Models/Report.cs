using System;
using System.ComponentModel.DataAnnotations;

namespace VPProject.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Issue is required")]
        public string Issue { get; set; }

        [Required(ErrorMessage = "Issue date is required")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Suggestion is required")]
        public string? Suggestion { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SuggestionDate { get; set; }
    }
}
