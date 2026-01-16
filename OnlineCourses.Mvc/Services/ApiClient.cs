using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using OnlineCourses.Mvc.Models;

namespace OnlineCourses.Mvc.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContext;

    public ApiClient(IConfiguration config, IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;

        _http = new HttpClient
        {
            BaseAddress = new Uri(config["Api:BaseUrl"]!)
        };
    }

    // -----------------------------
    // TOKEN
    // -----------------------------
    public void SetToken(string token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    // -----------------------------
    // AUTO REFRESH
    // -----------------------------
    private async Task<bool> TryRefreshTokenAsync()
    {
        var session = _httpContext.HttpContext!.Session;
        var refreshToken = session.GetString("refreshToken");

        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var response = await _http.PostAsJsonAsync(
            "auth/refresh",
            new { refreshToken }
        );

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadAsStringAsync();

        var tokens = JsonSerializer.Deserialize<TokenResponse>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        session.SetString("token", tokens!.Token);
        session.SetString("refreshToken", tokens.RefreshToken);

        SetToken(tokens.Token);

        return true;
    }

    // -----------------------------
    // GET (con auto-refresh)
    // -----------------------------
    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            var refreshed = await TryRefreshTokenAsync();
            if (!refreshed)
                return default;

            response = await _http.GetAsync(url);
        }

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    // -----------------------------
    // POST / PUT / DELETE
    // -----------------------------
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PostAsJsonAsync(url, body);
        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> PostAsync<TRequest>(string url, TRequest body)
    {
        var response = await _http.PostAsJsonAsync(url, body);
        return response.IsSuccessStatusCode;
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PutAsJsonAsync(url, body);
        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await _http.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
    
    // -----------------------------
// PATCH
// -----------------------------
    public async Task<bool> PatchAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, url);
        var response = await _http.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
    
   


}
