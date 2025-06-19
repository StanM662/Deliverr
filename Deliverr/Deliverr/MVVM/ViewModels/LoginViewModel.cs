using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Deliverr.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deliverr.ViewModels;
public partial class LoginViewModel : ObservableObject
{
    public List<Account> _users = new List<Account>()
    {
        new Account("admin", "admin", "Admin Details", "admin@example.com", new Image { Source = "zuyd_logo.png" }),
        new Account("Stan", "Stan123", "Stan Morreau", "stan@example.com", new Image { Source = "zuyd_logo.png" }),
        new Account("Sander", "Sander123", "Sander Kleijnen", "sander@example.com", new Image { Source = "zuyd_logo.png" }),
        new Account("Rick", "Rick123", "Rick Theunisz", "rick@example.com", new Image { Source = "zuyd_logo.png" }),
    };

    [ObservableProperty]
    string name;

    [ObservableProperty]
    string password;

    public Account LoggedInAccount { get; set; }

    [RelayCommand]
    async Task VerifyName()
    {
        if (_users.Any(u => u.Username == Name))
        {
            await Shell.Current.GoToAsync("WachtwoordLogin");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Username does not exist...", "OK");
        }
    }

    [RelayCommand]
    async Task VerifyPassword()
    {
        var account = _users.FirstOrDefault(u => u.Username == Name && u.Password == Password);

        if (account != null)
        {
            LoggedInAccount = account;
            UserSession.LoggedInUser = account.Username;
            UserSession.LoggedInAccount = account;

            await Shell.Current.GoToAsync("//WelkomPagina");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Password/Username combination is incorrect...", "OK");
        }
    }
}
