using System;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;

[assembly: ExportRendererAttribute(typeof(Xamore.Controls.Border), typeof(Xamore.Controls.iOS.Renderers.BorderRenderer))]

namespace Xamore.Controls.iOS.Renderers
{
    public class BorderRenderer : VisualElementRenderer<Border>
    {
        CALayer[] borderLayers = new CALayer[4];

        public BorderRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Border> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.SetupLayer();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName || e.PropertyName == Border.StrokeProperty.PropertyName || e.PropertyName == Border.WidthProperty.PropertyName || e.PropertyName == Border.HeightProperty.PropertyName)
            {
                this.SetupLayer();
            }
        }

        private void SetupLayer()
        {
            Layer.CornerRadius = (nfloat)Element.CornerRadius;
            if (Element.BackgroundColor != Color.Default)
            {
                Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();
            }
            else
            {
                Layer.BackgroundColor = UIColor.White.CGColor;
            }

            UpdateBorderLayer(BorderPosition.Left, (nfloat)Element.StrokeThickness.Left);
            UpdateBorderLayer(BorderPosition.Top, (nfloat)Element.StrokeThickness.Top);
            UpdateBorderLayer(BorderPosition.Right, (nfloat)Element.StrokeThickness.Right);
            UpdateBorderLayer(BorderPosition.Bottom, (nfloat)Element.StrokeThickness.Bottom);
            
            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }

        enum BorderPosition
        {
            Left,
            Top,
            Right,
            Bottom
        }

        void UpdateBorderLayer(BorderPosition borderPosition, nfloat thickness)
        {
            var borderLayer = borderLayers[(int)borderPosition];
            if (thickness <= 0)
            {
                if (borderLayer != null)
                {
                    borderLayer.RemoveFromSuperLayer();
                    borderLayers[(int)borderPosition] = null;
                }
            }
            else
            {
                if (borderLayer == null)
                {
                    borderLayer = new CALayer();
                    Layer.AddSublayer(borderLayer);
                    borderLayers[(int)borderPosition] = borderLayer;
                }

                switch (borderPosition)
                {
                    case BorderPosition.Left:
                        borderLayer.Frame = new CGRect(0, 0, thickness, Frame.Height);
                        break;
                    case BorderPosition.Top:
                        borderLayer.Frame = new CGRect(0, 0, Frame.Width, thickness);
                        break;
                    case BorderPosition.Right:
                        borderLayer.Frame = new CGRect(Frame.Width - thickness, 0, thickness, Frame.Height);
                        break;
                    case BorderPosition.Bottom:
                        borderLayer.Frame = new CGRect(0, Frame.Height - thickness, Frame.Width, thickness);
                        break;
                }

                var c = Element.Stroke;
                Debug.WriteLine("{0} {1} {2}", c.R, c.G, c.B);
                borderLayer.BackgroundColor = Element.Stroke.ToCGColor();
            }
        }
    }

    //public class BorderRenderer : ViewRenderer<Xamore.Controls.Border, System.Windows.Controls.Border>
    //{
    //    public BorderRenderer()
    //    {
    //        AutoPackage = false;
    //    }

    //    protected override void OnElementChanged(ElementChangedEventArgs<Border> e)
    //    {
    //        base.OnElementChanged(e);
    //        base.SetNativeControl(new System.Windows.Controls.Border());
    //        this.PackChild();
    //        this.UpdateBorder();
    //    }

    //    protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnElementPropertyChanged(sender, e);
    //        if (e.PropertyName == "Content")
    //        {
    //            this.PackChild();
    //        }
    //        else if (e.PropertyName == Frame.OutlineColorProperty.PropertyName || e.PropertyName == Frame.HasShadowProperty.PropertyName)
    //        {
    //            this.UpdateBorder();
    //        }
    //    }


    //    private void PackChild()
    //    {
    //        if (base.Element.Content == null)
    //        {
    //            return;
    //        }
    //        if (base.Element.Content.GetRenderer() == null)
    //        {
    //            base.Element.Content.SetRenderer(RendererFactory.GetRenderer(base.Element.Content));
    //        }
    //        var renderer = base.Element.Content.GetRenderer() as System.Windows.UIElement;
    //        base.Control.Child = renderer;
    //    }

    //    private void UpdateBorder()
    //    {
    //        base.Control.CornerRadius = new System.Windows.CornerRadius(Element.CornerRadius);
    //        base.Control.BorderBrush = Element.Stroke.ToBrush();
    //        base.Control.BorderThickness = Element.StrokeThickness.ToWinPhone();
    //    }
    //}
}

