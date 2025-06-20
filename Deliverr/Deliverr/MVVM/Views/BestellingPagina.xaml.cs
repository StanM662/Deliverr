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
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false, IsVisible = false });

            await vm.LoadInitialOrders();
        }

    }
}
