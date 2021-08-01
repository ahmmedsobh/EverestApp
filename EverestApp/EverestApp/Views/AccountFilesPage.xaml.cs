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
    public partial class AccountFilesPage : ContentPage
    {
        AccountFilesViewModel ViewModel = new AccountFilesViewModel(); 
        public AccountFilesPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}