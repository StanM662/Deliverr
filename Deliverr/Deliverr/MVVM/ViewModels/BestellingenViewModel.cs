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
        IsLoading = true;
        var orderList = await _apiService.GetOrdersAsync();
        var stateList = await _apiService.GetDeliveryStatesAsync();

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
                order.DeliveryStates.Add(new DeliveryState { State = 1 });
                Orders.Add(order);
            }
            
        }
        IsLoading = false;
    }



    private async Task StartDelivery(Order order)                                       // Start de levering van een specifieke bestelling via API                                  //
    {                                                                                   //                                                                                          //
        if (order == null) return;                                                      // Controleer of de order null is                                                           //
        try                                                                             //                                                                                          //
        {                                                                               //                                                                                          //
            var result = await _apiService.StartDelivery(order.Id);                     // Roep API aan om levering te starten                                                      //
            if (result.IsSuccessStatusCode)                                             // Als succesvol, verfris de order                                                          //
            {                                                                           //                                                                                          //
                var refreshedOrder = await _apiService.GetOrderByIdAsync(order.Id);     // Haal geüpdatete order op                                                                 //
                ReplaceOrderInCollection(refreshedOrder);                               // Vervang de oude order in de collectie met de nieuwe versie                               //
            }                                                                           //                                                                                          //
            else                                                                        //                                                                                          //
            {                                                                           //                                                                                          //
                Console.WriteLine($"Error starting delivery... {result.ReasonPhrase}");    // Toon foutmelding in console                                                              //
            }                                                                           //                                                                                          //
        }                                                                               //                                                                                          //
        catch (Exception ex)                                                            //                                                                                          //
        {                                                                               //                                                                                          //
            Console.WriteLine($"Start levering mislukt: {ex.Message}");                 // Toon foutmelding bij exceptie                                                            //
        }                                                                               //                                                                                          //
    }                                                                                   //                                                                                          //

    private async Task CompleteDelivery(Order order)                                    // Voltooi de levering van een specifieke bestelling via API                                //
    {                                                                                   //                                                                                          //
        if (order == null) return;                                                      // Controleer of de order null is                                                           //
        try                                                                             //                                                                                          //
        {                                                                               //                                                                                          //
            var result = await _apiService.CompleteDelivery(order.Id);                  // Roep API aan om levering te voltooien                                                    //
            if (result.IsSuccessStatusCode)                                             // Als succesvol, verfris de order                                                          //
            {                                                                           //                                                                                          //
                var refreshedOrder = await _apiService.GetOrderByIdAsync(order.Id);     // Haal geüpdatete order op                                                                 //
                ReplaceOrderInCollection(refreshedOrder);                               // Vervang de oude order in de collectie met de nieuwe versie                               //
            }                                                                           //                                                                                          //
            else                                                                        //                                                                                          //
            {                                                                           //                                                                                          //
                Console.WriteLine($"Error finishing delivery... {result.ReasonPhrase}");// Toon foutmelding in console                                                              //
            }                                                                           //                                                                                          //
        }                                                                               //                                                                                          //
        catch (Exception ex)                                                            //                                                                                          //
        {                                                                               //                                                                                          //
            Console.WriteLine($"Voltooien levering mislukt: {ex.Message}");             // Toon foutmelding bij exceptie                                                            //
        }                                                                               //                                                                                          //
    }                                                                                   //                                                                                          //

    private void ReplaceOrderInCollection(Order updatedOrder)                           // Vervang een order in de ObservableCollection met een nieuwe versie                       //
    {                                                                                   //                                                                                          //
        if (updatedOrder == null) return;                                               // Controleer of de nieuwe order null is                                                    //
        var existingOrder = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);        // Zoek de bestaande order met dezelfde ID                                                  //
        if (existingOrder != null)                                                      // Als de bestaande order gevonden is                                                       //
        {                                                                               //                                                                                          //
            var index = Orders.IndexOf(existingOrder);                                  // Haal de index van de bestaande order                                                     //
            Orders[index] = updatedOrder;                                               // Vervang de bestaande order met de nieuwe versie                                          //
        }                                                                               //                                                                                          //
    }                                                                                   //                                                                                          //

    public event PropertyChangedEventHandler? PropertyChanged;                          // Event voor property changes (voor databinding)                                           //
    protected void OnPropertyChanged(string propertyName) =>                            // Methode om het PropertyChanged event te triggeren                                        //
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));      // Notificeer listeners dat een property is veranderd                                       //
}                                                                                       //                                                                                          //
