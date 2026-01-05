namespace OnlineCourses.Application.Courses.Commands;

public record UpdateCourseCommand
(
    Guid Id,
    string Title
);