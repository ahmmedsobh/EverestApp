using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
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
                new Item(){Id="2",Text="الشحنات",Icon=FontAwesomeIcons.BoxOpen,IconColor="#056839",Url="MyAccountPage"},
                new Item(){Id="3",Text="الحسابات",Icon=FontAwesomeIcons.User,IconColor="#056839",Url="MyAccountPage"},
                new Item(){Id="4",Text="معلومات العميل",Icon=FontAwesomeIcons.InfoCircle,IconColor="#056839",Url="MyAccountPage"},
            };

            foreach(var item in items)
            {
                Items.Add(item);
            }

        }

        void FillMediaList()
        {
            var media = new List<Item>()
            {
                new Item(){Id="1",Icon=FontAwesomeIcons.FacebookF,IconColor="#3b5998"},
                new Item(){Id="2",Icon=FontAwesomeIcons.TwitterSquare,IconColor="#1da1f2"},
                new Item(){Id="3",Icon=FontAwesomeIcons.InstagramSquare,IconColor="#c32aa3"},
                new Item(){Id="4",Icon=FontAwesomeIcons.Telegram,IconColor="#0088cc"},
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



        async void OnArticleSelected(Article article)
        {
            try
            {
                if (article != null)
                {
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
                    await Shell.Current.GoToAsync($"///{item.Url}");
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
