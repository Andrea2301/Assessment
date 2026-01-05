using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Mvc.Models;

public class CourseViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public CourseStatus Status { get; set; }


}