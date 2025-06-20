using Deliverr.ViewModels;
namespace Deliverr;

public partial class WachtwoordLogin : ContentPage
{
	public WachtwoordLogin(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = true, IsVisible = true });
        Shell.SetTabBarIsVisible(this, false);

    }

}