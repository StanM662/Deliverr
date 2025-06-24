using Deliverr.ViewModels;
using Deliverr.Models;
using Deliverr.Views;

namespace Deliverr
{
    public partial class BestellingPagina : ContentPage
    {
        private BestellingenViewModel vm;
        private bool ordersLoaded = false;
        public BestellingPagina()
        {
            InitializeComponent();
            vm = new BestellingenViewModel();
            BindingContext = vm;
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false, IsVisible = false });
            if (ordersLoaded == false)
            {
                await vm.LoadInitialOrders(20);
                ordersLoaded = true;
            }
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is not Order order) return;
            if (order.DeliveryStates.Count == 2)
            {
                try
                {
                    var foto = await MediaPicker.CapturePhotoAsync();
                    if (foto == null)
                    {
                        await DisplayAlert("Fout", "Je moet een foto maken om te kunnen voltooien.", "OK");
                        return;
                    }

                    var stream = await foto.OpenReadAsync();
                    var completePagina = new CompletePagina();
                    completePagina.InitializeWithData(order, stream);
                    await Navigation.PushAsync(completePagina);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Fout", "Er is een fout opgetreden: " + ex.Message, "OK");
                }
            }

            else if (order.DeliveryStates.Count == 1)
            {
                await DisplayAlert("Fout", "Er is een fout opgetreden: " + "Je order is nog niet gestart", "OK");
            }

            else if (order.DeliveryStates.Count == 3)
            {
                await DisplayAlert("Fout", "Er is een fout opgetreden: " + "Je order is al voltooid", "OK");
            }
        }
    }
}
