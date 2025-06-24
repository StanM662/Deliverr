using System.Collections.ObjectModel;                                                   // Voor het gebruiken van een dynamische lijst die meldingen stuurt bij veranderingen       //
using System.ComponentModel;                                                            // Voor het implementeren van INotifyPropertyChanged (databinding support)                  //
using System.Runtime.CompilerServices;
using System.Windows.Input;                                                             // Voor ICommand (voor binding van knoppen aan methodes)                                    //
using Deliverr.Models;                                                                  // Voor toegang tot de Order klasse en andere modellen                                      //
namespace Deliverr.ViewModels;                                                          // Namespace voor ViewModels binnen het Deliverr project                                    //
public class BestellingenViewModel : INotifyPropertyChanged                             // ViewModel voor het beheren van bestellingen in de UI                                     //
{                                                                                       //                                                                                          //
    private readonly ApiService _apiService = new ApiService();
    private ObservableCollection<Order> _orders = new();
    private bool _isLoading;
    private int _currentId = 1;

    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set
        {
            if (_orders != value)
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }

    public ICommand LoadMoreOrdersCommand { get; }
    public ICommand StartDeliveryCommand { get; }
    public ICommand CompleteDeliveryCommand { get; }

    public BestellingenViewModel()
    {
        StartDeliveryCommand = new Command<Order>(async order => await StartDelivery(order));
        CompleteDeliveryCommand = new Command<Order>(async order => await CompleteDelivery(order));
        LoadMoreOrdersCommand = new Command(async () => await LoadNextOrders(20));
    }

    public async Task LoadInitialOrders(int orderCap)
    {
        await LoadNextOrders(orderCap); 
    }

    private async Task LoadNextOrders(int batchSize)
    {
        if (IsLoading) return;

        IsLoading = true;
        int addedCount = 0;

        while (addedCount < batchSize)
        {
            try
            {
                var order = await _apiService.GetOrderByIdAsync(_currentId);
                _currentId++;  // increment regardless of success or failure to avoid infinite loop

                if (order == null)
                {
                    // No order found for this ID, skip
                    continue;
                }

                var statesForOrder = order.DeliveryStates ?? new List<DeliveryState>();

                if (!statesForOrder.Any())
                {
                    // No states found - consider it Pending
                    order.DeliveryStates = new List<DeliveryState> { new DeliveryState { State = 1 } };
                    order.DeliveryStatus = "Pending";
                    Orders.Add(order);
                    addedCount++;
                    Console.WriteLine($"Added Pending order {order.Id} (no delivery states)");
                }
                else if (statesForOrder.Count == 1 && statesForOrder[0].State == 1)
                {
                    // Exactly one delivery state and it is Pending
                    order.DeliveryStatus = "Pending";
                    Orders.Add(order);
                    addedCount++;
                    Console.WriteLine($"Added Pending order {order.Id}");
                }
                else
                {
                    // Skip orders with multiple states or non-pending states
                    Console.WriteLine($"Skipped order {order.Id} with delivery states count: {statesForOrder.Count} or non-pending state.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading order with ID {_currentId}: {ex.Message}");
                _currentId++; // increment anyway to avoid infinite loop on failure
            }
        }

        IsLoading = false;
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
                order = updatedOrder;
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

        // ✅ Try to get the address
        string address = "";
        try
        {
            address = order.Customer.Address;

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new Exception("Customer.Address is empty or null.");
            }

            Console.WriteLine($"Successfully got order address: {address}");
        }
        catch (Exception e)
        {
            // Fallback address if something goes wrong
            address = "Nieuw Eyckholt 300, 6419 DJ Heerlen";
            Console.WriteLine($"Error encountered while getting address: {e.Message}. Using fallback: {address}");
        }

        // ✅ Launch Google Maps
        try
        {
            string encodedAddress = Uri.EscapeDataString(address);
            var url = $"https://www.google.com/maps/search/?api=1&query={encodedAddress}";
            Console.WriteLine($"Opening Google Maps to address: {url}");

            await Launcher.Default.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open Google Maps: {ex.Message}");
        }
    }


    public async Task CompleteDelivery(Order order)
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
}                                                                                       // Hoi Stan                                                                             //
