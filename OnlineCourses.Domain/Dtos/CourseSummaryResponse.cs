using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Domain.Dtos;

public class CourseSummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public CourseStatus Status { get; set; }
    public int TotalLessons { get; set; }
    public DateTime LastUpdateAt { get; set; }
}