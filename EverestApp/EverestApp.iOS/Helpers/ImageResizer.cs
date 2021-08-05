using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using EverestApp.Helpers;
using System.Drawing;
using CoreGraphics;
using EverestApp.iOS.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResizer))]
namespace EverestApp.iOS.Helpers
{
    public class ImageResizer : IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;
            var h = 800 * originalImage.Size.Height / originalImage.Size.Width;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)800, (int)h, 8,
                                                 4 * (int)800, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, 800,(float)h);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG(70).ToArray();
            }
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}