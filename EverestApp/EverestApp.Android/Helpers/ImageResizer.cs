using Android.Graphics;
using EverestApp.Droid.Helpers;
using EverestApp.Helpers;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResizer))]
namespace EverestApp.Droid.Helpers
{
    public class ImageResizer : IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            int h = 800 * originalImage.Height / originalImage.Width;
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)800, (int)h, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 70, ms);
                return ms.ToArray();
            }
        }
    }
}