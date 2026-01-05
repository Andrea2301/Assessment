using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;
using OnlineCourses.Infrastructure.Persistence.Data;

namespace OnlineCourses.Infrastructure.Persistence.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    }

    // LIST lessons by course (sorted)
    public async Task<IEnumerable<Lesson>> GetByCourseAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    // GET by Id
    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);
    }

    // CREATE
    public async Task AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
    }

    // UPDATE
    public Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        return Task.CompletedTask;
    }

    // DELETE (soft)
    public async Task SoftDeleteAsync(Guid id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null) return;

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;
    }

    // SAVE
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}