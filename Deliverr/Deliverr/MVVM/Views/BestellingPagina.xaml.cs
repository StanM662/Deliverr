using Deliverr.ViewModels;
using Deliverr.Models;
using Deliverr.Views;

namespace Deliverr
{
    public partial class BestellingPagina : ContentPage
    {
        private BestellingenViewModel vm;
        private ApiService _apiService = new ApiService();

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

            await vm.LoadInitialOrders();
        }

        private async void OnCompleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BindingContext is not Order order) return;

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


    }
}
