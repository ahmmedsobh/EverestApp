using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    [QueryProperty(nameof(ShipmentId), nameof(ShipmentId))]
    class ShipmentDetailsViewModel :BaseViewModel
    {
        public IShipmentService<Shipment> ShipmentService => DependencyService.Get<IShipmentService<Shipment>>();

        
        public ShipmentDetailsViewModel()
        {
            
        }

        string shipmentId;
        public string ShipmentId
        {
            get => shipmentId;
            set
            {
                SetProperty(ref shipmentId, value);
                GetFileDetails(value);
            }
        }

        Shipment shipment;
        public Shipment Shipment
        {
            get => shipment;
            set
            {
                SetProperty(ref shipment, value);
            }
        }


        async void GetFileDetails(string ShipmentId)
        {
            IsBusy = true;
            var shipments = await ShipmentService.GetShipmentsAsync();
            var shipment = shipments.FirstOrDefault(o => o.ID == ShipmentId);
            Shipment = new Shipment()
            {
                Index = shipment.Index,
                ID = shipment.ID,
                CustomerID = shipment.CustomerID,
                UploadDate = shipment.UploadDate,
                UpdateDate = shipment.UpdateDate,
            };
            IsBusy = false;
        }

        
    }
}
