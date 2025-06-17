using Deliverr.ViewModels;

namespace Deliverr;
public partial class LoginPagina : ContentPage
{
    public LoginPagina(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
