using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace Deliverr;

public partial class RoutePagina : ContentPage
{
    public RoutePagina()
    {
        InitializeComponent();
    }

    private async void OnToonRouteClicked(object sender, EventArgs e)
    {
        try
        {
            string startInput = StartEntry.Text?.Trim();
            string endInput = EndEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(startInput))
            {
                await DisplayAlert("Fout", "Startadres mag niet leeg zijn.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(endInput))
            {
                await DisplayAlert("Fout", "Bestemmingsadres mag niet leeg zijn.", "OK");
                return;
            }

            // Encode inputs for URL
            string encodedStart = Uri.EscapeDataString(startInput);
            string encodedEnd = Uri.EscapeDataString(endInput);

            // Google Maps directions URL with origin and destination
            var url = $"https://www.google.com/maps/dir/?api=1&origin={encodedStart}&destination={encodedEnd}";

            Console.WriteLine($"Opening Google Maps directions: {url}");

            await Launcher.Default.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open Google Maps: {ex.Message}");
            await DisplayAlert("Fout", $"Er is een fout opgetreden bij het openen van de route: {ex.Message}", "OK");
        }
    }
}
