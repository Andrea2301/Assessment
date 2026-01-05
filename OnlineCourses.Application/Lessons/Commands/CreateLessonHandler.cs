using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;

namespace OnlineCourses.Application.Lessons.Commands;

public class CreateLessonHandler
{
    private readonly ILessonRepository _repository;

    public CreateLessonHandler(ILessonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateLessonCommand command)
    {
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = command.CourseId,
            Title = command.Title,
            Order = command.Order,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(lesson);
        await _repository.SaveChangesAsync();

        return lesson.Id;
    }
}