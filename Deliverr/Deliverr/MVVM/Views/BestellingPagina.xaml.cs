using Deliverr.ViewModels;
using Deliverr.Models;
using Deliverr.Views;

namespace Deliverr
{
    public partial class BestellingPagina : ContentPage
    {
        private BestellingenViewModel vm;
        private ApiService _apiService = new ApiService();
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

            MessagingCenter.Subscribe<CompletePagina, Order>(this, "OrderCompleted", (sender, updatedOrder) =>
            {
                var oldOrder = vm.Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
                if (oldOrder != null)
                {
                    vm.Orders.Remove(oldOrder);
                    vm.Orders.Add(updatedOrder);
                }
            });

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
                    // 1. Foto maken
                    var foto = await MediaPicker.CapturePhotoAsync();
                    if (foto == null)
                    {
                        await DisplayAlert("Fout", "Je moet een foto maken om te kunnen voltooien.", "OK");
                        return;
                    }

                    var stream = await foto.OpenReadAsync();

                    // 2. Navigeer naar CompletePagina met order en fotoStream (zonder update)
                    var completePagina = new CompletePagina();
                    completePagina.InitializeWithData(order, stream);

                    // Navigeer en wacht op resultaat (bool) via TaskCompletionSource of MessagingCenter (hier eenvoudig via await PushAsync)
                    await Navigation.PushAsync(completePagina);

                    // Eventueel: refresh orders als update succesvol was in CompletePagina (zie daar)

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
