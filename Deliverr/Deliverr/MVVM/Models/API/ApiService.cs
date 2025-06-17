using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Deliverr.Models;
namespace Deliverr.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly APIKey _key = new();

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://51.137.100.120:5000/")
            };

            if (!_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
            {
                _httpClient.DefaultRequestHeaders.Add("ApiKey", _key.key);
            }
        }

        public async Task<List<Order>> GetOrdersAsync() =>
            await GetFromApi<List<Order>>("api/Order");

        public async Task<List<Order>> GetOrderByIdAsync(int id) =>
            await GetFromApi<List<Order>>($"api/Order/{id}");

        public async Task<Order> UpdateDeliveryState(int orderId, DeliveryStatesEnum newState)
        {
            var orders = await GetOrderByIdAsync(orderId);
            var order = orders.FirstOrDefault();
            if (order == null) return null;

            var deliveryService = new DeliveryService
            {
                Id = 1,
                Name = "Default Service"
            };

            var deliveryState = new DeliveryState
            {
                Id = 0, 
                OrderId = orderId,
                Order = order,
                State = (int)newState,
                DateTime = DateTime.UtcNow,
                DeliveryServiceId = deliveryService.Id,
                DeliveryService = deliveryService
            };

            return await PostToApi<Order>("api/DeliveryStates/UpdateDeliveryState", deliveryState);
        }

        public async Task<Order> StartDelivery(int orderId)
        {
            var orders = await GetOrderByIdAsync(orderId);
            var order = orders.FirstOrDefault();
            if (order == null) return null;

            var deliveryService = new DeliveryService
            {
                Id = 1,
                Name = "Default Service"
            };

            var deliveryState = new DeliveryState
            {
                Id = 0,
                OrderId = orderId,
                Order = order,
                State = (int)DeliveryStatesEnum.Shipping, 
                DateTime = DateTime.UtcNow,
                DeliveryServiceId = deliveryService.Id,
                DeliveryService = deliveryService
            };

            return await PostToApi<Order>("api/DeliveryStates/StartDelivery", deliveryState);
        }


        public async Task<Order> CompleteDelivery(int orderId)
        {
            var orders = await GetOrderByIdAsync(orderId);
            var order = orders.FirstOrDefault();
            if (order == null) return null;

            var deliveryService = new DeliveryService
            {
                Id = 1,
                Name = "Default Service"
            };

            var deliveryState = new DeliveryState
            {
                Id = 0,
                OrderId = orderId,
                Order = order,
                State = (int)DeliveryStatesEnum.Delivered,
                DateTime = DateTime.UtcNow,
                DeliveryServiceId = deliveryService.Id,
                DeliveryService = deliveryService
            };

            return await PostToApi<Order>("api/DeliveryStates/CompleteDelivery", deliveryState);
        }


        private async Task<T> GetFromApi<T>(string endpoint)
        {
            try
            {
                Debug.WriteLine($"GET: {endpoint}");

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response: {content}");

                return JsonSerializer.Deserialize<T>(content, _jsonOptions)
                    ?? Activator.CreateInstance<T>();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"GET error: {ex.Message}");
                return Activator.CreateInstance<T>();
            }
        }

        private async Task<T> PostToApi<T>(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                Debug.WriteLine($"POST: {endpoint}");
                Debug.WriteLine($"Payload: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response: {responseBody}");

                return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions)
                    ?? Activator.CreateInstance<T>();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"POST error: {ex.Message}");
                return Activator.CreateInstance<T>();
            }
        }
    }
}
