using System.Collections.ObjectModel;                                                   // Voor het gebruiken van een dynamische lijst die meldingen stuurt bij veranderingen       //
using System.ComponentModel;                                                            // Voor het implementeren van INotifyPropertyChanged (databinding support)                  //
using System.Windows.Input;                                                             // Voor ICommand (voor binding van knoppen aan methodes)                                    //
using Deliverr.Models;                                                                  // Voor toegang tot de Order klasse en andere modellen                                      //
namespace Deliverr.ViewModels;                                                          // Namespace voor ViewModels binnen het Deliverr project                                    //
public class BestellingenViewModel : INotifyPropertyChanged                             // ViewModel voor het beheren van bestellingen in de UI                                     //
{                                                                                       //                                                                                          //
    private readonly ApiService _apiService = new ApiService();                         // Instantie van de ApiService om data op te halen en te verzenden                          //
    private ObservableCollection<Order> _orders = new();                                // Verzameling van bestellingen die in de UI weergegeven wordt                              //
    private bool _isLoading;                                                            // Geeft aan of er op dit moment data geladen wordt                                         //

    public ObservableCollection<Order> Orders                                           // Publieke eigenschap voor binding van de bestellingen in de UI                            //
    {                                                                                   //                                                                                          //
        get => _orders;                                                                 // Geeft de huidige lijst terug                                                             //
        set                                                                             //                                                                                          //
        {                                                                               //                                                                                          //
            if (_orders != value)                                                       // Alleen wijzigen als de nieuwe waarde anders is                                           //
            {                                                                           //                                                                                          //
                _orders = value;                                                        // Wijzig de interne waarde                                                                 //
                OnPropertyChanged(nameof(Orders));                                      // Informeer de UI dat de waarde is aangepast                                               //
            }                                                                           //                                                                                          //
        }                                                                               //                                                                                          //
    }                                                                                   //                                                                                          //

    public bool IsLoading                                                               // Publieke eigenschap om te tonen of data geladen wordt                                    //
    {                                                                                   //                                                                                          //
        get => _isLoading;                                                              // Geeft de huidige status terug                                                            //
        set                                                                             //                                                                                          //
        {                                                                               //                                                                                          //
            if (_isLoading != value)                                                    // Alleen wijzigen als de waarde anders is                                                  //
            {                                                                           //                                                                                          //
                _isLoading = value;                                                     // Wijzig de interne waarde                                                                 //
                OnPropertyChanged(nameof(IsLoading));                                   // Informeer de UI dat de waarde is aangepast                                               //
            }                                                                           //                                                                                          //
        }                                                                               //                                                                                          //
    }                                                                                   //                                                                                          //

    public ICommand StartDeliveryCommand { get; }                                       // Command voor het starten van een levering                                                //
    public ICommand CompleteDeliveryCommand { get; }                                    // Command voor het voltooien van een levering                                              //
    public BestellingenViewModel()                                                      // Constructor van de ViewModel                                                             //
    {                                                                                   //                                                                                          //
        StartDeliveryCommand = new Command<Order>(async order =>                        // Initialiseer StartDeliveryCommand met async lambda                                       //
            await StartDelivery(order));                                                //                                                                                          //
        CompleteDeliveryCommand = new Command<Order>(async order =>                     // Initialiseer CompleteDeliveryCommand met async lambda                                    //
            await CompleteDelivery(order));                                             //                                                                                          //
    }                                                                                   //                                                                                          //                                                                                  //                                                                                          //

    public async Task LoadOrdersAsync()
    {
        if (Orders.Count >= 1)
        {
            return;
        }
        else
        {
            IsLoading = true;
            var ordersTask = _apiService.GetOrdersAsync();
            var statesTask = _apiService.GetDeliveryStatesAsync();

            await Task.WhenAll(ordersTask, statesTask);

            var orderList = ordersTask.Result;
            var stateList = statesTask.Result;

            for (int i = 0; i < orderList.Count; i++)
            {
                var order = orderList[i];
                DeliveryState? state = i < stateList.Count ? stateList[i] : null;
                order.DeliveryStates = new List<DeliveryState>();
                if (state != null)
                {
                    order.DeliveryStates.Add(state);
                }
                else
                {
                    DeliveryState ds = new DeliveryState { State = 1 };
                    order.DeliveryStates.Add(ds);
                    string _state = "Pending";
                    order.DeliveryStatus = _state;
                    Orders.Add(order);
                }

            }
            IsLoading = false;
        }
    }



    private async Task StartDelivery(Order order)
    {
        if (order == null) return;

        try
        {
            var result = await _apiService.StartDelivery(order.Id);
            if (result.IsSuccessStatusCode)
            {
                var updatedOrder = await _apiService.GetOrderByIdAsync(order.Id);
                ReplaceOrderInCollection(updatedOrder, "Shipping");
            }
            else
            {
                Console.WriteLine($"Start delivery failed: {result.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij starten levering: {ex.Message}");
        }
    }

    private async Task CompleteDelivery(Order order)
    {
        if (order == null) return;

        try
        {
            var result = await _apiService.CompleteDelivery(order.Id);
            if (result.IsSuccessStatusCode)
            {
                var updatedOrder = await _apiService.GetOrderByIdAsync(order.Id);
                ReplaceOrderInCollection(updatedOrder, "Delivered");
            }
            else
            {
                Console.WriteLine($"Complete delivery failed: {result.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij voltooien levering: {ex.Message}");
        }
    }

    private void ReplaceOrderInCollection(Order updatedOrder, string _state)
    {
        var existing = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
        if (existing != null)
        {
            var index = Orders.IndexOf(existing);
            updatedOrder.DeliveryStatus = _state;
            Orders[index] = updatedOrder;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;                          // Event voor property changes (voor databinding)                                           //
    protected void OnPropertyChanged(string propertyName) =>                            // Methode om het PropertyChanged event te triggeren                                        //
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));      // Notificeer listeners dat een property is veranderd                                       //
}                                                                                       //                                                                                          //
