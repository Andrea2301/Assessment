using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Domain.Dtos;
using OnlineCourses.Mvc.Services;
using TokenResponse = OnlineCourses.Mvc.Models.TokenResponse;

public class AuthController : Controller
{
    private readonly ApiClient _api;

    public AuthController(ApiClient api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _api.PostAsync<LoginRequest, TokenResponse>(
            "auth/login",
            request
        );

        if (result == null)
        {
            ModelState.AddModelError("", "Credenciales inválidas");
            return View();
        }

        // Guardar tokens en sesión
        HttpContext.Session.SetString("token", result.Token);
        HttpContext.Session.SetString("refreshToken", result.RefreshToken);

        _api.SetToken(result.Token);

        return RedirectToAction("Index", "Home");
    }
}