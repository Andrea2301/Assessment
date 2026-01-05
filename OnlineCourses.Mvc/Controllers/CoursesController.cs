using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Mvc.Models;
using OnlineCourses.Mvc.Services;

namespace OnlineCourses.Mvc.Controllers;

public class CoursesController : Controller
{
    private readonly ApiClient _api;

    public CoursesController(ApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        // Obtener token de sesión
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        // Llamada segura a la API
        var courses = await _api.GetAsync<List<CourseViewModel>>("courses");

        // Si es null, devolver lista vacía
        courses ??= new List<CourseViewModel>();

        return View(courses);
    }
}