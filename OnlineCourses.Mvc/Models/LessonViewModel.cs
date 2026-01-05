namespace OnlineCourses.Mvc.Models;

public class LessonViewModel
{
    public Guid Id { get; set; }   = Guid.Empty;
    public Guid CourseId { get; set; }
    public string Title { get; set; } = "";
    public int Order { get; set; }
}