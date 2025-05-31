using GestionContactosApi.Models;
using GestionContactosApi.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GestionContactosApi.Services
{
    public class ContactoService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _supabaseSettings;
        private readonly JsonSerializerOptions _jsonOptions;

        public ContactoService(HttpClient httpClient, IOptions<SupabaseSettings> options)
        {
            _httpClient = httpClient;
            _supabaseSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_supabaseSettings.Url);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("apikey", _supabaseSettings.ApiKey);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<Contacto>> ObtenerTodos()
        {
            var response = await _httpClient.GetAsync("/contactos?select=*");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Contacto>>(json, _jsonOptions)!;
        }

        public async Task<Contacto?> ObtenerPorId(int id)
        {
            var response = await _httpClient.GetAsync($"/api/contactos?id=eq.{id}&select=*");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<List<Contacto>>(json, _jsonOptions);
            return lista?.FirstOrDefault();
        }

        public async Task<bool> Agregar(Contacto contacto)
        {
            var content = new StringContent(JsonSerializer.Serialize(contacto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/contacto", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(int id)
        {
            var response = await _httpClient.DeleteAsync($"/contactos?id=eq.{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Editar(Contacto contacto)
        {
            var content = new StringContent(JsonSerializer.Serialize(contacto), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Patch, $"/contactos?id=eq.{contacto.Id}")
            {
                Content = content
            };

            request.Headers.Add("Prefer", "return=representation");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
