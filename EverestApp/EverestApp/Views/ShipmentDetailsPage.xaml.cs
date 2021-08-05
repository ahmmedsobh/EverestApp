using EverestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EverestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShipmentDetailsPage : ContentPage
    {
        public ShipmentDetailsPage()
        {
            InitializeComponent();
            BindingContext = new ShipmentDetailsViewModel();
        }
    }
}