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
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = true, IsVisible = false });
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
    }

}
