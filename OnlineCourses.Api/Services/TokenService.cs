using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OnlineCourses.Domain.Entities;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    // ACCESS TOKEN (1 minuto)
    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var expires = DateTime.UtcNow.AddMinutes(1);

        Console.WriteLine(
            $"[JWT] Access token generado | Usuario: {user.Email} | Expira: {expires:HH:mm:ss}"
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // REFRESH TOKEN (1 minuto, como necesitas para el profe)
    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(1),
            IsRevoked = false
        };
    }
}