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
    class ShipmentViewModel:BaseViewModel
    {
        public IShipmentService<Shipment> ShipmentService => DependencyService.Get<IShipmentService<Shipment>>();


        private Shipment _selectedShipment;

        public ObservableCollection<Shipment> Shipments { get; }
        public Command LoadShipmentsCommand { get; }
        public Command<Shipment> ShipmentTapped { get; }

        public ShipmentViewModel()
        {
            Shipments = new ObservableCollection<Shipment>();
            LoadShipmentsCommand = new Command(async () => await ExecuteLoadShipmentsCommand());

            ShipmentTapped = new Command<Shipment>(OnShipmentSelected);

        }

        async Task ExecuteLoadShipmentsCommand()
        {

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;

            try
            {
                Shipments.Clear();
                var shipments = await ShipmentService.GetShipmentsAsync(true);
                foreach (var item in shipments)
                {
                    Shipments.Add(item);
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
            SelectedShipment = null;
        }

        public Shipment SelectedShipment
        {
            get => _selectedShipment;
            set
            {
                SetProperty(ref _selectedShipment, value);
                OnShipmentSelected(value);
            }
        }



        async void OnShipmentSelected(Shipment shipment)
        {
            if (shipment == null)
                return;

            SelectedShipment = null;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ShipmentDetailsPage)}?{nameof(ShipmentDetailsViewModel.ShipmentId)}={shipment.ID}");
        }

       

    }
}
