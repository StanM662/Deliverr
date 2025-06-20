using Deliverr.Models;
using Deliverr.ViewModels;


namespace Deliverr.Views;

public partial class CompletePagina : ContentPage
{
    private Order _order;
    private Stream _fotoStream;
    private BestellingenViewModel vm;
    public CompletePagina()
    {
        InitializeComponent();
        vm = new BestellingenViewModel();
    }

    public void InitializeWithData(Order order, Stream fotoStream)
    {
        _order = order;
        _fotoStream = fotoStream;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_fotoStream != null)
        {
            Made_Picture.Source = ImageSource.FromStream(() => _fotoStream);
        }
    }

    private async void OnMaakFotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                Made_Picture.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fout", "Kon geen foto maken: " + ex.Message, "OK");
        }
    }

    private async void OnCompleteClicked(object sender, EventArgs e)
    {
        await vm.CompleteDelivery(_order);
        await Shell.Current.GoToAsync("BestellingPagina");
    }
}
