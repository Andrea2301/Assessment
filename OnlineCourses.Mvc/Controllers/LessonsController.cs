using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Mvc.Models;
using OnlineCourses.Mvc.Services;

namespace OnlineCourses.Mvc.Controllers;

public class LessonsController : Controller
{
    private readonly ApiClient _api;

    public LessonsController(ApiClient api) => _api = api;

    // LISTAR LECCIONES
    public async Task<IActionResult> Index(Guid courseId)
    {
        Console.WriteLine($"[LessonsController] CourseId recibido: {courseId}");

        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        var lessons = await _api.GetAsync<List<LessonViewModel>>(
            $"courses/{courseId}/lessons"
        ) ?? new List<LessonViewModel>();

        Console.WriteLine($"[LessonsController] Lecciones obtenidas: {lessons.Count}");

        ViewBag.CourseId = courseId;
        return View(lessons);
    }

    // CREAR / EDITAR (POST dinámico)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrEdit(LessonViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Index), new { courseId = model.CourseId });

        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        if (model.Id == Guid.Empty)
        {
            // Crear nueva lección
            await _api.PostAsync<LessonViewModel, object>(
                $"courses/{model.CourseId}/lessons",
                model
            );
        }
        else
        {
            // Editar lección existente
            await _api.PutAsync<object, object>(
                $"courses/{model.CourseId}/lessons/{model.Id}",
                new { title = model.Title, order = model.Order }
            );
        }

        return RedirectToAction(nameof(Index), new { courseId = model.CourseId });
    }

    // ELIMINAR
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid courseId, Guid id)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        await _api.DeleteAsync($"courses/{courseId}/lessons/{id}");
        return RedirectToAction(nameof(Index), new { courseId });
    }

    // MOVER (UP/DOWN)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Move(Guid courseId, Guid id, bool up)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        await _api.PutAsync<object, object>(
            $"courses/{courseId}/lessons/{id}/move?up={up}",
            new { }
        );

        return RedirectToAction(nameof(Index), new { courseId });
    }
}
