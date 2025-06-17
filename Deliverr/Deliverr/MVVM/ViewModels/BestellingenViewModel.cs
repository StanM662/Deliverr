using System.Collections.ObjectModel;
using System.ComponentModel;
using Deliverr.Models;
using Deliverr.ViewModels;

namespace Deliverr.ViewModels;
public class BestellingenViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService = new ApiService();
    private ObservableCollection<Order> _bestellingen;

    public ObservableCollection<Order> Bestellingen
    {
        get => _bestellingen;
        set
        {
            _bestellingen = value;
            OnPropertyChanged(nameof(Bestellingen));
        }
    }

    public BestellingenViewModel()
    {
        LoadBestellingen();
    }

    private async void LoadBestellingen()
    {
        var list = await _apiService.GetBestellingenAsync();
        Bestellingen = new ObservableCollection<Order>(list);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
