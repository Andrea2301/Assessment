namespace OnlineCourses.Domain.Dtos;

public record CreateCourseRequest
{
    public string Title { get; set; }
    
    public string Description { get; set; } = "";
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = "General";
}

public record UpdateCourseRequest(
    string Title
);

