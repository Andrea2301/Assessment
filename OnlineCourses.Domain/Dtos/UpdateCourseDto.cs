namespace OnlineCourses.Domain.Dtos;

public class UpdateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = "";
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = "General";
   
}