using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;

namespace OnlineCourses.Application.Courses.Commands;

public class CreateCourseHandler(ICourseRepository repository)
{
    public async Task<Guid> Handle(CreateCourseCommand command)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Status = CourseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await repository.AddAsync(course);
        await repository.SaveChangesAsync();

        return course.Id;
    }
}