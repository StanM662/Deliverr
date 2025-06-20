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
        LoadMoreOrdersCommand = new Command(async () => await LoadNextOrders());
    }

    public async Task LoadInitialOrders()
    {
        await LoadNextOrders(); 
    }

    private async Task LoadNextOrders()
    {
        if (IsLoading) return;

        IsLoading = true;

        for (int i = 0; i < 20; i++)
        {
            try
            {
                var order = await _apiService.GetOrderByIdAsync(_currentId);
                if (order != null)
                {
                    // Set default state or delivery status
                    order.DeliveryStates = new List<DeliveryState> { new DeliveryState { State = 1 } };
                    order.DeliveryStatus = "Pending";
                    Orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order with ID {_currentId} not found or error: {ex.Message}");
            }

            _currentId++;
        }

        IsLoading = false;
    }
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
