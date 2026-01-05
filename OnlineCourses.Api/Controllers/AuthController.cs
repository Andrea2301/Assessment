
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Dtos;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Persistence.Data;

namespace OnlineCourses.Api.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] AppDbContext context)
    {
        var exists = await context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (exists)
            return BadRequest("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] AppDbContext context,
        [FromServices] IConfiguration config)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var token = JwtTokenGenerator.Generate(user, config);

        return Ok(new { token });
    }

}