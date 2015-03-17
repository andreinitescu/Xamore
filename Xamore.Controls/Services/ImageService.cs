using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamore.Controls.Services
{
    public interface ImageService
    {
        Task<Size> Measure(ImageSource imageSource);
    }
}
