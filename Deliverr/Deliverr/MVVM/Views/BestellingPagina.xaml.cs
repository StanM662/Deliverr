using Deliverr.ViewModels;
namespace Deliverr;

public partial class BestellingPagina : ContentPage
{
    public BestellingPagina()
    {
        InitializeComponent();
        BindingContext = new BestellingenViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BestellingenViewModel vm)
        {
            await vm.LoadBestellingenAsync();
        }
    }
}
