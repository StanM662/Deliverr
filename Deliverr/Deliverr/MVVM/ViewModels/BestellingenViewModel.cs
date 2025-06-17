using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Deliverr.Models;
using Microsoft.Maui.Controls;

namespace Deliverr.ViewModels
{
    public class BestellingenViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        private ObservableCollection<Order> _orders = new();
        private bool _isLoading;

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

        public ICommand StartDeliveryCommand { get; }
        public ICommand UpdateDeliveryCommand { get; }
        public ICommand CompleteDeliveryCommand { get; }
        public ICommand CancelDeliveryCommand { get; }

        public BestellingenViewModel()
        {
            StartDeliveryCommand = new Command<Order>(async order => await StartDelivery(order));
            UpdateDeliveryCommand = new Command<Order>(async order => await UpdateDelivery(order));
            CompleteDeliveryCommand = new Command<Order>(async order => await CompleteDelivery(order));
            CancelDeliveryCommand = new Command<Order>(async order => await CancelDelivery(order));
        }

        public async Task LoadOrdersAsync()
        {
            IsLoading = true;
            try
            {
                var list = await _apiService.GetOrdersAsync();
                Orders = new ObservableCollection<Order>(list);
            }
            catch (Exception ex)
            {
                DisplayError("Fout bij het laden van bestellingen: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task StartDelivery(Order order)
        {
            if (order == null) return;

            try
            {
                var updatedOrder = await _apiService.StartDelivery(order.Id);
                //ReplaceOrderInCollection(updatedOrder);
            }
            catch (Exception ex)
            {
                DisplayError("Start levering mislukt: " + ex.Message);
            }
        }

        private async Task CompleteDelivery(Order order)
        {
            if (order == null) return;

            try
            {
                var updatedOrder = await _apiService.CompleteDelivery(order.Id);
                ReplaceOrderInCollection(updatedOrder);
            }
            catch (Exception ex)
            {
                DisplayError("Voltooien levering mislukt: " + ex.Message);
            }
        }

        private async Task UpdateDelivery(Order order)
        {
            if (order == null) return;

            try
            {
                var updatedOrder = await _apiService.UpdateDeliveryState(order.Id, DeliveryStatesEnum.Shipping);
                ReplaceOrderInCollection(updatedOrder);
            }
            catch (Exception ex)
            {
                DisplayError("Update levering mislukt: " + ex.Message);
            }
        }

        private async Task CancelDelivery(Order order)
        {
            if (order == null) return;

            try
            {
                var updatedOrder = await _apiService.UpdateDeliveryState(order.Id, DeliveryStatesEnum.Cancelled);
                ReplaceOrderInCollection(updatedOrder);
            }
            catch (Exception ex)
            {
                DisplayError("Annuleren levering mislukt: " + ex.Message);
            }
        }

        private void ReplaceOrderInCollection(Order updatedOrder)
        {
            if (updatedOrder == null) return;

            var existingOrder = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);
            if (existingOrder != null)
            {
                var index = Orders.IndexOf(existingOrder);
                Orders[index] = updatedOrder;
            }
        }

        private void DisplayError(string message)
        {
            Console.WriteLine(message);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
