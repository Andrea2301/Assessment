using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Domain.Repositories;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetByCourseAsync(Guid courseId);
    Task<Lesson?> GetByIdAsync(Guid id);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task SoftDeleteAsync(Guid id);
    Task SaveChangesAsync();
}