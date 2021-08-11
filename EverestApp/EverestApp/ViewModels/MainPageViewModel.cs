using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using EverestApp.Views;
using Plugin.LocalNotification;
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
        public IMessageService<Message> MessageService => DependencyService.Get<IMessageService<Message>>();
        public IMessagesFileService<Message> MessagesFileService => DependencyService.Get<IMessagesFileService<Message>>();

        public ObservableCollection<Article> Articles { get; }
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<Item> Media { get; }
        public Command LoadArticlesCommand { get; }
        public Command<Article> ArticleTapped { get; }
        public Command OpenMessagesPageCommand { get; }

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
            ShowNotifications();
            OpenMessagesPageCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(MessagesPage));
            });
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
                new Item(){Id="2",Text="الشحنات",Icon=FontAwesomeIcons.BoxOpen,IconColor="#056839",Url="ShipmentsPage"},
                new Item(){Id="3",Text="الحسابات",Icon=FontAwesomeIcons.User,IconColor="#056839",Url="AccountFilesPage"},
                new Item(){Id="4",Text="معلومات العميل",Icon=FontAwesomeIcons.InfoCircle,IconColor="#056839",Url="MyAccountPage"},
                new Item(){Id="5",Text="تعليمات الشحن",Icon=FontAwesomeIcons.ShippingFast,IconColor="#056839",Url="ShipmentInfoPage"},
                new Item(){Id="6",Text="الحسابات البنكية",Icon=FontAwesomeIcons.DollarSign,IconColor="#056839",Url="BankAccountsPage"},
                new Item(){Id="7",Text="الرسائل",Icon=FontAwesomeIcons.CommentMedical,IconColor="#056839",Url="MessagesPage"},
                new Item(){Id="8",Text="تواصل معنا",Icon=FontAwesomeIcons.Envelope,IconColor="#056839",Url="ContactPage"},
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

        string newMessagesCount;
        public string NewMessagesCount
        {
            get => newMessagesCount;
            set
            {
                SetProperty(ref newMessagesCount, value);
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

        async void ShowNotifications()
        {
            try
            {
                var MessagesModelView = (await MessagesFileService.UpdateMessagesFile());
                var Count = MessagesModelView.NewMessagesCount;
                if (Count > 0)
                {
                    NewMessagesCount = Count.ToString();
                    var notification = new NotificationRequest
                    {
                        BadgeNumber = Count,
                        Description = $"وصلتك {Count} رسائل جديده",
                        Title = "الاشعارات",
                        ReturningData = $"وصلتك {Count} رسائل جديده",
                        NotificationId = 1337,
                        Schedule = { NotifyTime = DateTime.Now.AddSeconds(8) },
                    };

                    await NotificationCenter.Current.Show(notification);

                }
                else
                {
                    NewMessagesCount = "لايوجد";
                }
            }
            catch
            {

            }
            
        }
    }
}
