namespace OnlineCourses.Application.Lessons.Commands;

public class CreateLessonCommand
{
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
}