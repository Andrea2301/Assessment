using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Domain.Dtos;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;
using OnlineCourses.Infrastructure.Persistence.Data;

namespace OnlineCourses.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly ICourseRepository _repository;
    private readonly AppDbContext _context;

    public CoursesController(
        ICourseRepository repository,
        AppDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    // LIST
    [HttpGet]
    public async Task<IActionResult> Get(
        CourseStatus? status,
        int page = 1,
        int pageSize = 10)
    {
        var courses = await _repository.GetAsync(status, page, pageSize);
        return Ok(courses);
    }

    // GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _repository.GetByIdAsync(id);
        if (course == null) return NotFound();
        return Ok(course);
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseRequest dto)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Status = CourseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = course.Id }, null);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        course.Title = dto.Title;
        course.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE (soft)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUBLISH
    [HttpPatch("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // UNPUBLISH
    [HttpPatch("{id}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
