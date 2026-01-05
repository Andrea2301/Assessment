namespace OnlineCourses.Domain.Dtos;

public record CreateLessonRequest
{
    
public Guid CourseId { get; set; }
public string Title { get; set; } = string.Empty;
public int Order { get; set; }
}




