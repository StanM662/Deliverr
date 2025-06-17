using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Deliverr.ViewModels;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {

    }

    [RelayCommand]
    async Task Tap(string s)
    {
        if (!string.IsNullOrWhiteSpace(s))
        {
            await Shell.Current.GoToAsync($"/{s}");

        }
    }

}

