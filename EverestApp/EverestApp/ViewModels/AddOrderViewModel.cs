using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Drawing;

namespace EverestApp.ViewModels
{
    class AddOrderViewModel:BaseViewModel
    {
        public IOrderService<Order> OrderService => DependencyService.Get<IOrderService<Order>>();
        public IImageResizer ImageResizer => DependencyService.Get<IImageResizer>();
        
        public Command UploadImageCommand { get; }
        public Command TakeImageCommand { get; }
        public Command SendOrderCommand { get; }
        public AddOrderViewModel()
        {
            UploadImageCommand = new Command(ExecuteUploadImageCommand);
            TakeImageCommand = new Command(ExecuteTakeImageCommand);
            SendOrderCommand = new Command(ExecuteSendOrderCommand);
        }

        string message;
        public string Message 
        {
            get => message; 
            set
            {
                SetProperty(ref message,value);
            }
        }

        

        byte[] imageAsBytes;
        public byte[] ImageAsBytes
        {
            get => imageAsBytes;
            set
            {
                SetProperty(ref imageAsBytes, value);
            }
        }

        async void ExecuteUploadImageCommand()
        {
            var file = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return;

            SetImageFullPath(file);
        }

        

        async void ExecuteTakeImageCommand()
        {
            var file = await MediaPicker.CapturePhotoAsync();

            if (file == null)
                return;

            SetImageFullPath(file);
        }

        async void SetImageFullPath(FileResult file)
        {
            var ImageExtnsion = file.FileName.Split('.')[1];
            if (ImageExtnsion != "jpg")
            {
                Message = "الامتداد المسموح به jpg";
                return;
            }

            Message = "";
            MemoryStream memoryStream = new MemoryStream();
            var streem = await file.OpenReadAsync();
            streem.CopyTo(memoryStream);
            
            //resize image 
            var ResizedImage = ImageResizer.ResizeImage(memoryStream.ToArray(), 500, 500);

            ImageAsBytes = new byte[0];
            ImageAsBytes = ResizedImage;
        }

        async void ExecuteSendOrderCommand()
        {
            if (ImageAsBytes == null)
            {
                Message = "قم برفع صورة";
                return;
            }

            if (ImageAsBytes.Length == 0)
            {
                Message = "قم برفع صورة";
                return;
            }

            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            IsBusy = true;
            var result = await OrderService.AddOrderAsync(ImageAsBytes);
            if (result)
            {
                ImageAsBytes = new byte[0];
                await Shell.Current.GoToAsync("..");
                await Shell.Current.DisplayAlert("ارسال طلب", "تم ارسال طلبك", "موافق");
            }
            else
            {
                await Shell.Current.DisplayAlert("ارسال طلب", "حدث خطأ ، حاول مرة اخرى", "موافق");
            }
        }



    }
}
