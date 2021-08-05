using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EverestApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BankAccountsPage : ContentPage
    {
        public BankAccountsPage()
        {
            InitializeComponent();
            Img1.Source = ImageSource.FromResource("EverestApp.Resources.Images.Qnp.png", typeof(BankAccountsPage).GetTypeInfo().Assembly);
            Img2.Source = ImageSource.FromResource("EverestApp.Resources.Images.Qnp.png", typeof(BankAccountsPage).GetTypeInfo().Assembly);
            Img3.Source = ImageSource.FromResource("EverestApp.Resources.Images.Qnp.png", typeof(BankAccountsPage).GetTypeInfo().Assembly);
            Img4.Source = ImageSource.FromResource("EverestApp.Resources.Images.Plasten.png", typeof(BankAccountsPage).GetTypeInfo().Assembly);
        }
    }
}