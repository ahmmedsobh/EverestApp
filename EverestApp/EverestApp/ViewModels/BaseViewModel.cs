using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IConectionService connectionService = DependencyService.Get<IConectionService>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string BackgroundImg
        {
            get => "EverestApp.Resources.Images.Background.jpg";
        }

        public string BackgroundImg2
        {
            get => "EverestApp.Resources.Images.Background2.jpg";
        }

        public string BackgroundImg3
        {
            get => "EverestApp.Resources.Images.Background3.jpg";
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
