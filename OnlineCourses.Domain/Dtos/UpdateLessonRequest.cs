namespace OnlineCourses.Domain.Dtos;

public class UpdateLessonRequest
{
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}