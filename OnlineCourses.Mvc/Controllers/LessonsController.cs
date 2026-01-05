using Microsoft.AspNetCore.Mvc;

namespace OnlineCourses.Mvc.Controllers;

public class LessonsController :  Controller
{
    public IActionResult Index() => View();
    public IActionResult Create() => View();
    public IActionResult Edit() => View();
}