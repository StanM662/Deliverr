using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Deliverr.Models
{
    public partial class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly APIKey key = new APIKey();

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://51.137.100.120:5000/"),
                Timeout = TimeSpan.FromSeconds(1)  // Time-out van 10 seconden
            };

            // Header toevoegen als deze nog niet bestaat
            if (!_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
            {
                _httpClient.DefaultRequestHeaders.Add("ApiKey", key.key);
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            try
            {
                Debug.WriteLine("Start API call naar /api/Order ...");
                var response = await _httpClient.GetAsync("api/Order");

                Debug.WriteLine($"Response status code: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response content: {content}");

                var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return orders ?? new List<Order>();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"API request failed: {e.Message}");
                return new List<Order>();
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("Request timed out.");
                return new List<Order>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Order>();
            }
        }
    }
}
