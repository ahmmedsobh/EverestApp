using EverestApp.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.Converters
{
    class StringToImageSourceConverter : IValueConverter
    {
        public IImageResizer ImageResizer => DependencyService.Get<IImageResizer>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                byte[] bytes;
                if (value is byte[])
                {
                    bytes = value as byte[];
                }
                else if((value as string).Contains("EverestApp.Resources.Images"))
                {
                    return ImageSource.FromResource($"{value}");
                }
                else
                {
                     bytes = File.ReadAllBytes(value.ToString());
                }
                //var ResizedImage = ImageResizer.ResizeImage(bytes,100,100);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            else
            {
                return null;
            }
        }

        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
