using OnlineCourses.Domain.Entities;

namespace OnlineCourses.Domain.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id); //get by id
    Task<IEnumerable<Course>> GetAsync(   //pagination
        CourseStatus? status,
        int page,
        int pageSize);

    Task AddAsync(Course course);
    
    Task UpdateAsync(Course course);

    Task SoftDeleteAsync(Guid id);
    Task SaveChangesAsync();
}