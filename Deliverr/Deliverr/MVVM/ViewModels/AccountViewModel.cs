using System.ComponentModel;
using Deliverr.Models;
using Microsoft.Maui.Controls;

namespace Deliverr.ViewModels;

public class AccountViewModel : INotifyPropertyChanged
{
    private string naam;
    private string details;
    private string email;
    private Image profilePicture;
    private List<string> bestellingen;

    public string Naam
    {
        get => naam;
        set
        {
            if (naam != value)
            {
                naam = value;
                OnPropertyChanged(nameof(Naam));
            }
        }
    }

    public string Details
    {
        get => details;
        set
        {
            if (details != value)
            {
                details = value;
                OnPropertyChanged(nameof(Details));
            }
        }
    }

    public string Email
    {
        get => email;
        set
        {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public Image ProfilePicture
    {
        get => profilePicture;
        set
        {
            if (profilePicture != value)
            {
                profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }
    }

    public List<string> Bestellingen
    {
        get => bestellingen;
        set
        {
            if (Bestellingen != value)
            {
                Bestellingen = value;
                OnPropertyChanged(nameof(Bestellingen));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public AccountViewModel(Account account)
    {
        Naam = account.Username;
        Details = account.Details;
        Email = account.Email;
        ProfilePicture = account.ProfilePicture;
    }
}
