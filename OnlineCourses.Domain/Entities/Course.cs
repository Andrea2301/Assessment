namespace OnlineCourses.Domain.Entities;

public class Course
{
    public Guid Id  { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Level { get; set; } = "Beginner";
    public string Category { get; set; } = "General";

    public CourseStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Lesson> Lessons { get; set; } = new();
}