using Microsoft.Maui.Media;

namespace Deliverr
{
    public partial class RoutePagina : ContentPage
    {
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
<!DOCTYPE html>
<html>
<head>
  <meta charset='utf-8'/>
  <title>Route Kaart</title>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css'/>
</head>
<body>
  <div id='map' style='width:100vw; height:400px;'></div>
  <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
  <script>
    var map = L.map('map').setView([49.417, 8.684], 14);
    L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(map);

    var route = {routeCoordinaten};
    var latlngs = route.map(function(c) {{ return [c[1], c[0]]; }});
    var polyline = L.polyline(latlngs, {{color:'blue'}}).addTo(map);
    map.fitBounds(polyline.getBounds());
  </script>
</body>
</html>";
            KaartView.Source = new HtmlWebViewSource { Html = html };
        }
    }
}
