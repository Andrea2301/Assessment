using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Application.Courses.Queries;

public class GetCourses
{
    public CourseStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}