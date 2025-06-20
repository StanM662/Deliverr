using Deliverr.ViewModels;
using Microsoft.Maui.Controls;

namespace Deliverr
{
    public partial class BestellingPagina : ContentPage
    {
        private BestellingenViewModel vm;

        public BestellingPagina()
        {
            InitializeComponent();
            vm = new BestellingenViewModel();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadOrdersAsync();
        }

        private async void OnRouteButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoutePage());
        }
    }

    public class RoutePage : ContentPage
    {
        private readonly Route route;

        public RoutePage()
        {
            route = new Route();
            Content = new Label { Text = "Route Page" };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await route.FetchAndDisplayRouteAsync();
        }
    }
}
