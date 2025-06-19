using System.ComponentModel;
using Deliverr.Models;
using Microsoft.Maui.Controls;

namespace Deliverr.ViewModels;

public class AccountViewModel : INotifyPropertyChanged
{
    private string user_name;
    private string user_details;
    private string user_email;
    private Image user_profile_picture;
    private List<string> user_orders;

    public string Name
    {
        get => user_name;
        set
        {
            if (user_name != value)
            {
                user_name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Details
    {
        get => user_details;
        set
        {
            if (user_details != value)
            {
                user_details = value;
                OnPropertyChanged(nameof(Details));
            }
        }
    }

    public string Email
    {
        get => user_email;
        set
        {
            if (user_email != value)
            {
                user_email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public Image ProfilePicture
    {
        get => user_profile_picture;
        set
        {
            if (user_profile_picture != value)
            {
                user_profile_picture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }
    }

    public List<string> Orders
    {
        get => user_orders;
        set
        {
            if (user_orders != value)
            {
                user_orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public AccountViewModel(Account account)
    {
        Name = account.Username;
        Details = account.Details;
        Email = account.Email;
        ProfilePicture = account.ProfilePicture;
    }
}
