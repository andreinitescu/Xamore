using System;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms.Platform.WinPhone;
using Xamore.Controls.WinPhone;
using Xamore.Controls.WinPhone.Renderers;
using System.Diagnostics;
using System.Windows.Controls;

[assembly: ExportRendererAttribute(typeof(Xamore.Controls.Border), typeof(Xamore.Controls.WinPhone.Renderers.BorderRenderer))]

namespace Xamore.Controls.WinPhone.Renderers
{
	public class BorderRenderer : ViewRenderer<Xamore.Controls.Border, System.Windows.Controls.Border>
    {
        public BorderRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Border> e)
        {
            base.OnElementChanged(e);
            SetNativeControl(new System.Windows.Controls.Border());
            PackChild();
            UpdateBorder();
        }

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Content")
            {
                PackChild();
            }
            else if (e.PropertyName == Border.StrokeProperty.PropertyName || 
                     e.PropertyName == Border.StrokeThicknessProperty.PropertyName ||
                     e.PropertyName == Border.CornerRadiusProperty.PropertyName)
            {
                UpdateBorder();
            }
		}

        // the base class is setting the background to the renderer when Control is null
        protected override void UpdateBackgroundColor()
        {
            if (Control != null)
            {
                Control.Background = (this.Element.BackgroundColor != Color.Default ? this.Element.BackgroundColor.ToBrush() : base.Background);
            }
        }

        private void PackChild()
        {
            if (Element.Content == null)
            {
                return;
            }
            if (Element.Content.GetRenderer() == null)
            {
                Element.Content.SetRenderer(RendererFactory.GetRenderer(Element.Content));
            }
            var renderer = Element.Content.GetRenderer() as System.Windows.UIElement;
            Control.Child = renderer;
        }

        private void UpdateBorder()
        {
            Control.CornerRadius = new System.Windows.CornerRadius(Element.CornerRadius);
            Control.BorderBrush = Element.Stroke.ToBrush();
            Control.BorderThickness = Element.StrokeThickness.ToWinPhone();
        }
    }
}

