using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;
using OnlineCourses.Infrastructure.Persistence.Data;

namespace OnlineCourses.Infrastructure.Persistence.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    // READ by Id
    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    // READ list (pagination + filter)
    public async Task<IEnumerable<Course>> GetAsync(
        CourseStatus? status,
        int page,
        int pageSize)
    {
        var query = _context.Courses
            .Where(c => !c.IsDeleted);

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // CREATE
    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
    }

    // UPDATE
    public Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        return Task.CompletedTask;
    }

    // DELETE (soft)
    public async Task SoftDeleteAsync(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return;

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;
    }

    // SAVE changes :)
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}