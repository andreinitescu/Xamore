using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamore.Controls.Services;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Xamore.Controls.Droid.Services
{
    public class ImageServiceDroid : ImageService
    {
        public static void Initialize()
        {
            DependencyService.Register<ImageServiceDroid>();
        }

        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }

        #region ImageService implementation

        public async Task<Size> Measure(ImageSource imageSource)
        {
			var bmp = await GetHandler(imageSource).LoadImageAsync(imageSource, Forms.Context);
            return new Xamarin.Forms.Size(bmp.Width, bmp.Height);
        }

        #endregion
    }
}