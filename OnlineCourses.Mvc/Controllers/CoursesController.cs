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

    public async Task<IActionResult> Index(int? status, int page = 1)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        var url = $"courses?page={page}&pageSize=10";

        if (status.HasValue)
            url += $"&status={status.Value}";

        var courses = await _api.GetAsync<List<CourseViewModel>>(url)
                      ?? new List<CourseViewModel>();

        return View(courses);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        return View(new CourseViewModel());
    }

    // POST: Courses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        var payload = new
        {
            title = model.Title
        };

        await _api.PostAsync("courses", payload);
        
        return RedirectToAction(nameof(Index));
    }
    
    // ✅ GET: Courses/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        var course = await _api.GetAsync<CourseViewModel>($"courses/{id}");
        return View(course);
    }

    // ✅ POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CourseViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        await _api.PutAsync<CourseViewModel, CourseViewModel>($"courses/{id}", model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        await _api.PutAsync<object, object>(
            $"courses/{id}/toggle-status",
            new { }
        );

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var token = HttpContext.Session.GetString("token");
        _api.SetToken(token!);

        await _api.DeleteAsync($"courses/{id}");
        return RedirectToAction(nameof(Index));
    }
}