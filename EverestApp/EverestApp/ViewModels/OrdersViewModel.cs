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

        public ObservableCollection<Order> Orders { get; }
        public Command LoadOrdersCommand { get; }
        public Command<Order> OrderTapped { get; }
        public Command AddOrderCommand { get; }

        public OrdersViewModel()
        {
            Title = "Browse";
            Orders = new ObservableCollection<Order>();
            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());

            OrderTapped = new Command<Order>(OnOrderSelected);

            AddOrderCommand = new Command(ExecuteAddOrderCommand);
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
                    var order = GetStatusInfo(item);
                    Orders.Add(order);
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

        Order GetStatusInfo(Order order)
        {
            //order.Satus = "6";
            switch(order.Satus)
            {
                case "1":
                    order.StatusTitle = "تم الارسال";
                    order.StatusColor = "#fce83a";
                    order.StatusIcon = FontAwesomeIcons.PaperPlane;
                    break;
                case "2":
                    order.StatusTitle = "تم العرض";
                    order.StatusColor = "#ffb302";
                    order.StatusIcon = FontAwesomeIcons.Eye;
                    break;
                case "3":
                    order.StatusTitle = "تم الاستلام";
                    order.StatusColor = "#2dccff";
                    order.StatusIcon = FontAwesomeIcons.SatelliteDish;
                    break;
                case "4":
                    order.StatusTitle = "قيد التنفيذ";
                    order.StatusColor = "#ff3838";
                    order.StatusIcon = FontAwesomeIcons.Spinner;
                    break;
                case "5":
                    order.StatusTitle = "تم الشحن";
                    order.StatusColor = "#56f000";
                    order.StatusIcon = FontAwesomeIcons.ShippingFast;
                    break;
                case "6":
                    order.StatusTitle = "تم التوصيل";
                    order.StatusColor = "#9ea7ad";
                    order.StatusIcon = FontAwesomeIcons.Truck;
                    break;
                case "7":
                    order.StatusTitle = "تم التسليم";
                    order.StatusColor = "#056839";
                    order.StatusIcon = FontAwesomeIcons.ClipboardCheck;
                    break;
            }

            return order;
        }

    }
}
