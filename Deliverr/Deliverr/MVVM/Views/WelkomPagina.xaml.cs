using Deliverr.ViewModels;
namespace Deliverr;

public partial class WelkomPagina : ContentPage
{
    public WelkomPagina()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false, IsVisible = false });
    }

}
