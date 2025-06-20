using Deliverr.Models;  
using Deliverr.ViewModels;

namespace Deliverr
{
    public partial class AccountPagina : ContentPage
    {
        public AccountPagina()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Account account = UserSession.LoggedInAccount ?? new Account
            {
                Username = "Gast",
                Password = "",
                Details = "",
                Email = "",
                ProfilePicture = new Image { Source = "zuyd_logo.png" }
            };

            var viewModel = new AccountViewModel(account);
            BindingContext = viewModel;
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false, IsVisible = false });

        }
    }
}
