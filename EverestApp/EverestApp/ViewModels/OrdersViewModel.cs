using EverestApp.Models;
using EverestApp.Services;
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

        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }
        public Command UploadOrderCommand { get; }
        public Command<Order> OrderTapped { get; }

        public OrdersViewModel()
        {
            Title = "Browse";
            Orders = new ObservableCollection<Order>();
            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());

            OrderTapped = new Command<Order>(OnOrderSelected);

            UploadOrderCommand = new Command(OnUploadOrder);
        }

        async Task ExecuteLoadOrdersCommand()
        {
            IsBusy = true;

            try
            {
                Orders.Clear();
                var orders = await OrderService.GetOrdersAsync(true);
                foreach (var item in orders)
                {
                    Orders.Add(item);
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

        private async void OnUploadOrder(object obj)
        {
            IsBusy = true;
            var result = await OrderService.AddOrderAsync();
            if(result)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert("ارسال طلب","تم ارسال طلبك","موافق");
            }
            else
            {
                await Shell.Current.DisplayAlert("ارسال طلب", "حدث خطأ يرجى المحاولة", "موافق");
            }
        }

        async void OnOrderSelected(Order order)
        {
            if (order == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

    }
}
