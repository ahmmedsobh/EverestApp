using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Helpers
{
    public interface IImageResizer
    {
         byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
