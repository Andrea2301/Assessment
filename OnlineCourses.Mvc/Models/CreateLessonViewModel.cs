namespace OnlineCourses.Mvc.Models;

public class CreateLessonViewModel
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = "";
    public int Order { get; set; }
}