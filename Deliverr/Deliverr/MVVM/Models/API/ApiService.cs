using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Deliverr.Models;

namespace Deliverr.Models
{
    public partial class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string APIKey = "e325173d - 1143 - 4959 - 85dd-bdf8dc8f3f73";
        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://51.137.100.120:5000/")
            };
            _httpClient.DefaultRequestHeaders.Add($"ApiKey", "{APIKey}");
        }

        public async Task<List<DeliveryService>> GetDeliveryServicesAsync()
        {
            var response = await _httpClient.GetAsync("api/DeliveryServices");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var services = JsonSerializer.Deserialize<List<DeliveryService>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return services ?? new List<DeliveryService>();
        }

        public async Task<DeliveryService> GetDeliveryServiceByApiKeyAsync(string apiKey)
        {
            var response = await _httpClient.GetAsync($"api/DeliveryServices/{apiKey}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var service = JsonSerializer.Deserialize<DeliveryService>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return service;
        }

        public async Task<bool> StartDeliveryAsync(StartDeliveryRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/DeliveryStates/StartDelivery", content);

            return response.IsSuccessStatusCode;
        }

        public class StartDeliveryRequest
        {
            public int OrderId { get; set; }
            public string DeliveryPerson { get; set; }
            
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/Order");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return orders ?? new List<Order>();
        }


        public async Task<List<Order>> GetBestellingenAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/bestellingen");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var bestellingen = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return bestellingen ?? new List<Order>();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"API request failed: {e.Message}");
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
