using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class MainPageViewModel:BaseViewModel
    {
        public Customer Customer { get; set; }
        private Article _selectedArticle;
        private Item _selectedItem;
        private Item _selectedMedia;
        IMainService MainService => DependencyService.Get<IMainService>();

        public ObservableCollection<Article> Articles { get; }
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<Item> Media { get; }
        public Command LoadArticlesCommand { get; }
        public Command<Article> ArticleTapped { get; }
        public MainPageViewModel()
        {
            GetCustomerDate();
            Articles = new ObservableCollection<Article>();
            Items = new ObservableCollection<Item>();
            Media = new ObservableCollection<Item>();
            LoadArticlesCommand = new Command(async () => await ExecuteLoadArticlesCommand());
            ArticleTapped = new Command<Article>(OnArticleSelected);
            FillItemsList();
            FillMediaList();
        }

        async Task ExecuteLoadArticlesCommand()
        {
            IsBusy = true;

            try
            {
                Articles.Clear();
                var articles = await MainService.GetArticlesAysnc(true);
                articles = articles.OrderByDescending(a => a.id).Take(3).ToList();
                foreach (var item in articles)
                {
                    Articles.Add(item);
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

       


        void FillItemsList()
        {
            var items = new List<Item>()
            {
                new Item(){Id="1",Text="الطلبيات",Icon=FontAwesomeIcons.ListAlt,IconColor="#056839",Url="OrdersPage"},
                new Item(){Id="2",Text="الشحنات",Icon=FontAwesomeIcons.BoxOpen,IconColor="#056839",Url=""},
                new Item(){Id="3",Text="الحسابات",Icon=FontAwesomeIcons.User,IconColor="#056839",Url="AccountFilesPage"},
                new Item(){Id="4",Text="معلومات العميل",Icon=FontAwesomeIcons.InfoCircle,IconColor="#056839",Url="MyAccountPage"},
            }; 

            foreach (var item in items)
            {
                Items.Add(item);
            }

        }

        void FillMediaList()
        {
            var media = new List<Item>()
            {
                new Item(){Id="1",Icon="EverestApp.Resources.Images.facebook.png",IconColor="#3b5998",Url="https://www.facebook.com/EverestExport"},
                new Item(){Id="2",Icon="EverestApp.Resources.Images.twitter.png",IconColor="#1da1f2",Url="https://twitter.com/ExportEverest"},
                new Item(){Id="3",Icon="EverestApp.Resources.Images.instagram.png",IconColor="#c32aa3",Url="https://www.instagram.com/everest_export/"},
                new Item(){Id="4",Icon="EverestApp.Resources.Images.telegram.png",IconColor="#0088cc",Url="https://telegram.me/everstexport"},
            };

            foreach (var item in media)
            {
                Media.Add(item);
            }

        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedArticle = null;
        }

        public Article SelectedArticle
        {
            get => _selectedArticle;
            set
            {
                SetProperty(ref _selectedArticle, value);
                OnArticleSelected(value);
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public Item SelectedMedia
        {
            get => _selectedMedia;
            set
            {
                SetProperty(ref _selectedMedia, value);
                OnMediaSelected(value);
            }
        }



        async void OnArticleSelected(Article article)
        {
            try
            {
                if (article != null)
                {
                    _selectedArticle = null;
                    var FullLink = $"https://www.everestexport.net{article.link}";
                    await Browser.OpenAsync(FullLink, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void OnItemSelected(Item item)
        {
            try
            {
                if (item != null)
                {
                    SelectedItem = null;
                    await Shell.Current.GoToAsync($"{item.Url}");
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void OnMediaSelected(Item item)
        {
            try
            {
                if (item != null)
                {
                    SelectedMedia = null;
                    await Browser.OpenAsync(item.Url);
                }
            }
            catch (Exception ex)
            {

            }
        }


        void GetCustomerDate()
        {
            Customer = new Customer();
            Customer.ID = Preferences.Get("ID", "");
            Customer.Code = Preferences.Get("Code", "");
            Customer.Name = Preferences.Get("Name", "");
            Customer.Password = Preferences.Get("Password", "");
            Customer.Info = Preferences.Get("Info", "");
        }
    }
}
