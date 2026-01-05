using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Domain.Dtos;

public class CourseSearchResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public CourseStatus Status { get; set; }
    public DateTime UpdateAt { get; set; }
    
}