using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class OrdersViewModel:BaseViewModel
    {
        public IOrderService<Order> OrderService => DependencyService.Get<IOrderService<Order>>();


        private Order _selectedOrder;

        public ObservableCollection<Category> Categories { get; }
        public Command LoadCategoriesCommand { get; }
        public Command<Order> OrderTapped { get; }
        public Command AddOrderCommand { get; }

        public OrdersViewModel()
        {
            Title = "Browse";
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadOrdersCommand());

            OrderTapped = new Command<Order>(OnOrderSelected);

            AddOrderCommand = new Command(ExecuteAddOrderCommand);
        }

        async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;

            try
            {
                Categories.Clear();
                var categories = await OrderService.GetOrdersByCategoryAsync(true);
                foreach (var item in categories)
                {
                    Categories.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedOrder = null;
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                OnOrderSelected(value);
            }
        }

        

        async void OnOrderSelected(Order order)
        {
            if (order == null)
                return;

            SelectedOrder = null;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?{nameof(OrderDetailsViewModel.OrderId)}={order.ID}");
        }

        async void ExecuteAddOrderCommand()
        {
            await Shell.Current.GoToAsync(nameof(AddOrderPage));
        }

       

    }
}
