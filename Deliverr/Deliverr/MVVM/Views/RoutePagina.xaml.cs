using Deliverr.Models;

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
            string startInput = StartEntry.Text;
            string endInput = EndEntry.Text;

            if (TryParseCoordinates(startInput, out double[] startCoords) &&
                TryParseCoordinates(endInput, out double[] endCoords))
            {
                ToonKaartMetRoute(startInput, endInput);
            }
            else
            {
                await DisplayAlert("Fout", "Voer geldige coördinaten in (bijv. 50.53284, 5.44237)", "OK");
            }
        }
        catch (Exception _e)
        {
            await DisplayAlert("Fout", "Er is een fout opgetreden: " + _e.Message, "OK");
        }
    }

    private bool TryParseCoordinates(string input, out double[] coords)
    {
        coords = null;
        var parts = input.Split(',');
        if (parts.Length != 2)
            return false;

        if (double.TryParse(parts[0].Trim(), out double lat) &&
            double.TryParse(parts[1].Trim(), out double lon))
        {
            coords = new double[] { lat, lon };
            return true;
        }
        return false;
    }

    private void ToonKaartMetRoute(string start, string end)
    {
        string routeCoordinaten = $"[{FormatCoords(start)}, {FormatCoords(end)}]";

        string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
              <meta charset='utf-8'/>
              <title>Route Kaart</title>
              <meta name='viewport' content='width=device-width, initial-scale=1.0'>
              <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css'/>
              <style>body, html, #map {{margin:0;padding:0;height:100%;}}</style>
            </head>
            <body>
              <div id='map'></div>
              <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
              <script>
                var route = {routeCoordinaten};
                var map = L.map('map').setView(route[0], 13);
                L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(map);

                var latlngs = route.map(function(c) {{ return [c[0], c[1]]; }});
                var polyline = L.polyline(latlngs, {{ color: 'blue' }}).addTo(map);

                L.marker(latlngs[0]).addTo(map).bindPopup('Start').openPopup();
                L.marker(latlngs[1]).addTo(map).bindPopup('End');

                map.fitBounds(polyline.getBounds());
              </script>
            </body>
            </html>";

        KaartView.Source = new HtmlWebViewSource { Html = html };
    }

    private string FormatCoords(string coord)
    {
        var parts = coord.Split(',');
        return $"[{parts[0].Trim()}, {parts[1].Trim()}]";
    }
}
