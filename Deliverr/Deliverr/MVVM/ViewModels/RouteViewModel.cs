using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

public class Route
{
    public async Task FetchAndDisplayRouteAsync()
    {
        await GetRouteAsync();
    }

    private static async Task GetRouteAsync()
    {
        string apiKey = "5b3ce3597851110001cf6248b28ab8aef0a34ed7bb44064af40ff08c";
        string start = "50.881314, 5.959227";
        string end = "50.929034, 5.817771";

        string url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={apiKey}&start={start}&end={end}";

        using var client = new HttpClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString); 

            // Route polyline ophalen
            var coordinates = json["features"][0]["geometry"]["coordinates"];

            // Instructies ophalen
            var steps = json["features"][0]["properties"]["segments"][0]["steps"];
            foreach (var step in steps)
            {
                string instruction = (string)step["instruction"];
                double distance = (double)step["distance"];
                // Toon de instructie in je app
                Console.WriteLine($"{instruction} ({distance} meter)");
            }
        }
        else
        {
            Console.WriteLine($"Fout: {response.StatusCode}");
        }
    }
}
