using System.Net.Http.Headers;
using System.Text.Json;

namespace OnlineCourses.Mvc.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(IConfiguration config)
    {
        _http = new HttpClient
        {
            BaseAddress = new Uri(config["Api:BaseUrl"]!)
        };
    }

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
    // Métodos genéricos seguros
    // -----------------------------

    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return default; // o lanzar excepción si prefieres

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PostAsJsonAsync(url, body);

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest body)
    {
        var response = await _http.PutAsJsonAsync(url, body);

        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await _http.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}
