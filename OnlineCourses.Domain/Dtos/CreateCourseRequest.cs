namespace OnlineCourses.Domain.Dtos;

public record CreateCourseRequest
{
    public string Title { get; set; }
}

public record UpdateCourseRequest(
    string Title
);

