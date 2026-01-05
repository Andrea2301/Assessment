using Microsoft.AspNetCore.Mvc;

namespace OnlineCourses.Mvc.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index() => View();
}