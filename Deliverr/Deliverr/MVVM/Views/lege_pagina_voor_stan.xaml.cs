<<<<<<< HEAD
using Microsoft.Maui.Media;
=======
using System.Formats.Tar;

namespace Deliverr;
>>>>>>> fc62b28c17c43bf56a43d393eb5a4426765e6581

namespace Deliverr
{
    public partial class RoutePagina : ContentPage
    {
<<<<<<< HEAD
        public RoutePagina()
        {
            InitializeComponent();
            ToonKaartMetRoute();
        }

        private async void OnMaakFotoClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    Made_Picture.Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", "Kon geen foto maken: " + ex.Message, "OK");
            }
        }

        private void ToonKaartMetRoute()
        {
            // Vervang deze polyline door je echte route/polyline
            string routeCoordinaten = "[[8.681495,49.41461],[8.687872,49.420318]]";
            string html = $@"
=======
        InitializeComponent();
        ToonKaartMetRoute("50.53284, 5.44237", "50.52538, 5.57355");
    }

        private void OnToonRouteClicked(object sender, EventArgs e)
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
                DisplayAlert("Fout", "Voer geldige coördinaten in (bijv. 50.53284, 5.44237)", "OK");
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
>>>>>>> fc62b28c17c43bf56a43d393eb5a4426765e6581
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <title>Route Kaart</title>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css'/>
</head>
<body>
  <div id='map' style='width:100vw; height:100vh;'></div>
  <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
  <script>
    var route = {routeCoordinaten};
    var map = L.map('map').setView(route[0], 13);
    L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(map);
<<<<<<< HEAD

    var route = {routeCoordinaten};
    var latlngs = route.map(function(c) {{ return [c[1], c[0]]; }});
    var polyline = L.polyline(latlngs, {{color:'blue'}}).addTo(map);
=======
    var polyline = L.polyline(route, {{ color: 'blue' }}).addTo(map);
>>>>>>> fc62b28c17c43bf56a43d393eb5a4426765e6581
    map.fitBounds(polyline.getBounds());
  </script>
</body>
</html>";
            KaartView.Source = new HtmlWebViewSource { Html = html };
        }
    }

    private string FormatCoords(string coord)
        {
            var parts = coord.Split(',');
            return $"[{parts[0].Trim()}, {parts[1].Trim()}]";
        }
    }
