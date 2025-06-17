using Deliverr.ViewModels;
namespace Deliverr;

public partial class BestellingPagina : ContentPage
{
    public BestellingPagina(PageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}