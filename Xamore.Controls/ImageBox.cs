using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamore.Controls.Services;

namespace Xamore.Controls
{
    public class ImageBox : Image
    {
        Size? imageSize;

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Source")
            {
                UpdateImageSize();
            }
        }

        protected override SizeRequest OnSizeRequest(Double widthConstraint, Double heightConstraint)
        {
            if (imageSize == null)
            {
                return new SizeRequest();
            }

            if (Double.IsInfinity(heightConstraint))
            {
                heightConstraint = widthConstraint * imageSize.Value.Width / imageSize.Value.Height;
            }
            else
            {
                widthConstraint = heightConstraint * imageSize.Value.Width / imageSize.Value.Height;
            }
            return new SizeRequest(new Size(widthConstraint, heightConstraint));
        }

        async void UpdateImageSize()
        {
            imageSize = null;
            imageSize = await DependencyService.Get<ImageService>().Measure(Source);
            InvalidateMeasure();
        }
    }
}
