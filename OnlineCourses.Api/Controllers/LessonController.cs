using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Domain.Dtos;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Repositories;

[ApiController]
[Route("api/courses/{courseId}/lessons")]
public class LessonsController : ControllerBase
{
    private readonly ILessonRepository _lessonRepository;

    public LessonsController(ILessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    // LIST
    [HttpGet]
    public async Task<IActionResult> Get(Guid courseId)
    {
        var lessons = await _lessonRepository.GetByCourseAsync(courseId);
        return Ok(lessons);
    }

    // GET by Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null) return NotFound();
        return Ok(lesson);
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid courseId,
        CreateLessonRequest dto)
    {
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId, 
            Title = dto.Title,
            Order = dto.Order,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _lessonRepository.AddAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { courseId, id = lesson.Id },
            null
        );
    }



    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid courseId,
        Guid id,
        [FromBody] UpdateLessonRequest dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);

        if (lesson == null || lesson.CourseId != courseId)
            return NotFound();

        lesson.Title = dto.Title;
        lesson.Order = dto.Order;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return NoContent();
    }


    // DELETE (soft)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _lessonRepository.SoftDeleteAsync(id);
        await _lessonRepository.SaveChangesAsync();

        return NoContent();
    }
    [HttpPatch("{id}/move")]
    public async Task<IActionResult> Move(
        Guid courseId,
        Guid id,
        [FromQuery] bool up)
    {
        var lessons = (await _lessonRepository.GetByCourseAsync(courseId)).ToList();
        var lesson = lessons.FirstOrDefault(l => l.Id == id);

        if (lesson == null) return NotFound();

        var index = lessons.IndexOf(lesson);
        var swapIndex = up ? index - 1 : index + 1;

        if (swapIndex < 0 || swapIndex >= lessons.Count)
            return BadRequest("Ivalid Move");

        var other = lessons[swapIndex];

        (lesson.Order, other.Order) = (other.Order, lesson.Order);

        await _lessonRepository.SaveChangesAsync();
        return NoContent();
    }

}