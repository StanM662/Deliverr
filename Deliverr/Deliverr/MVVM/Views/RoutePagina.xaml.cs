namespace Deliverr;

public partial class RoutePagina : ContentPage
{
    public RoutePagina()
    {
        InitializeComponent();
        ToonKaartMetRoute();
    }

    private void ToonKaartMetRoute()
    {
        // Voorbeeld route: Heidelberg centrum naar Heidelberg station (vervang met je eigen polyline!)
        string routeCoordinaten = "[[50.53284, 5.44237], [50.52538, 5.57355]]"; // [lat, lon] pairs
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

    var route = [[50.53284, 5.44237], [50.52538, 5.57355]];
    var latlngs = route.map(function(c) {{ return [c[1], c[0]]; }});
    var route = [[50.53284, 5.44237], [50.52538, 5.57355]];
    var polyline = L.polyline(route, {{color: 'blue' }}).addTo(map);
    map.fitBounds(polyline.getBounds());

  </script>
</body>
</html>";

        KaartView.Source = new HtmlWebViewSource { Html = html };
    }
}
