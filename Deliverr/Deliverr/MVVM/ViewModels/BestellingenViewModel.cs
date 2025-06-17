using System.Collections.ObjectModel;
using System.ComponentModel;
using Deliverr.Models;
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
        // We laden nu niet direct hier, maar in de pagina OnAppearing
    }

    public async Task LoadBestellingenAsync()
    {
        var list = await _apiService.GetOrdersAsync();
        Bestellingen = new ObservableCollection<Order>(list);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
