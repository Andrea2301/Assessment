using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Domain.Dtos;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Persistence.Data;
//using OnlineCourses.Api.Services;

namespace OnlineCourses.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService, ILogger<AuthController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    // -----------------------------
    // REGISTER
    // -----------------------------
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);

        if (exists)
            return BadRequest("User already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // -----------------------------
    // LOGIN
    // -----------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] AppDbContext context,
        [FromServices] TokenService tokenService)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken(user.Id);

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return Ok(new
        {
            token = accessToken,
            refreshToken = refreshToken.Token
        });
    }


    // -----------------------------
    // REFRESH TOKEN
    // -----------------------------
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r =>
                r.Token == request.RefreshToken &&
                !r.IsRevoked);

        if (refreshToken == null)
            return Unauthorized("Invalid refresh token");

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
            return Unauthorized("Refresh token expired");

        refreshToken.IsRevoked = true;

        var user = refreshToken.User;

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        _logger.LogInformation("""
                                REFRESH TOKEN EJECUTADO
                                Usuario : {Email}
                                Minuto   : {Time}
                                """,
            user.Email,
            DateTime.UtcNow.ToString("mm:ss"));


        return Ok(new
        {
            token = newAccessToken,
            refreshToken = newRefreshToken.Token
        });
    }



    // -----------------------------
    // LOGOUT
    // -----------------------------
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshTokenRequest request)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == request.RefreshToken);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}
