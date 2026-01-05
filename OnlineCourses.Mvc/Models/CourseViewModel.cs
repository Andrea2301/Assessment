using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Mvc.Models;

public class CourseViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; }
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = "General";
    public CourseStatus Status { get; set; }


}