using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    class OrderDetailsViewModel:BaseViewModel
    {
        public IOrderService<Order> OrderService => DependencyService.Get<IOrderService<Order>>();

        public OrderDetailsViewModel()
        {

        }

        string orderId;
        public string OrderId 
        {
            get => orderId;
            set
            {
                SetProperty(ref orderId,value);
                GetOrderDetails(value);
            }
        }

        Order order;
        public Order Order 
        {
            get => order;
            set
            {
                SetProperty(ref order, value);
            }
        }


        async void GetOrderDetails(string OrderId)
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;
            var orders = await OrderService.GetOrdersAsync();
            var order = orders.FirstOrDefault(o=>o.ID == OrderId);
            
            Order = new Order() 
            {
                Index = order.Index,
                ID = order.ID,
                CustomerID = order.CustomerID,
                UploadedDate = order.UploadedDate,
                UpdatedDate = order.UpdatedDate,
                StatusTitle = order.StatusTitle,
            };

            IsBusy = false;
        }

        Order GetStatusInfo(Order order)
        {
            
            switch (order.Satus)
            {
                case "1":
                    order.StatusTitle = "تم الارسال";
                    order.StatusColor = "#fce83a";
                    break;
                case "2":
                    order.StatusTitle = "تم العرض";
                    order.StatusColor = "#ffb302";
                    break;
                case "3":
                    order.StatusTitle = "تم الاستلام";
                    order.StatusColor = "#2dccff";
                    break;
                case "4":
                    order.StatusTitle = "قيد التنفيذ";
                    order.StatusColor = "#ff3838";
                    break;
                case "5":
                    order.StatusTitle = "تم الشحن";
                    order.StatusColor = "#56f000";
                    break;
                case "6":
                    order.StatusTitle = "تم التوصيل";
                    order.StatusColor = "#9ea7ad";
                    break;
                case "7":
                    order.StatusTitle = "تم التسليم";
                    order.StatusColor = "#056839";
                    break;
            }

            return order;
        }

    }
}
