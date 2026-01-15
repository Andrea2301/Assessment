using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Mvc.Services;
using OnlineCourses.Mvc.Models; // ✅ ESTE ERA EL PROBLEMA

public class LoginController : Controller
{
    private readonly ApiClient _api;

    public LoginController(ApiClient api)
    {
        _api = api;
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(string email, string password)
    {
        var response = await _api.PostAsync<object, TokenResponse>(
            "auth/login",
            new { email, password }
        );

        if (response == null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }

        HttpContext.Session.SetString("token", response.Token);
        HttpContext.Session.SetString("refreshToken", response.RefreshToken);

        return RedirectToAction("Index", "Courses");
    }
}