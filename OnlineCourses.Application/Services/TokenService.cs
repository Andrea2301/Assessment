using Microsoft.Extensions.Logging;

public class TokenService
{
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public string GenerateAccessToken(Guid userId)
    {
        var expires = DateTime.UtcNow.AddMinutes(1);

        _logger.LogInformation("""
                               ────────────────────────────────────────
                                AUTH
                               Access token generado
                               UsuarioId: {UserId}
                               Expira: {Expires}
                               ────────────────────────────────────────
                               """,
            userId,
            expires.ToString("HH:mm:ss"));

        return "jwt_generado";
    }
}