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

       

    }
}
