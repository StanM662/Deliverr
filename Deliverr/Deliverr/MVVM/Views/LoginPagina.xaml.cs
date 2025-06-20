using Deliverr.ViewModels;
namespace Deliverr;

public partial class LoginPagina : ContentPage
{
    public LoginPagina(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false, IsVisible = false });
    }
}
