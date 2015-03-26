using System;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Linq;
using Android.Graphics;

namespace Xamore.Controls.Droid.Renderers
{
	public static class BorderRendererVisual
	{
		public static void UpdateBackground(Border border, Android.Views.View view)
		{
			var strokeThickness = border.StrokeThickness;
			var context = view.Context;

			// create stroke drawable
			GradientDrawable strokeDrawable = null;

			// if thickness exists, set stroke drawable stroke and radius
			if(strokeThickness.HorizontalThickness + strokeThickness.VerticalThickness > 0) {
				strokeDrawable = new GradientDrawable ();
				strokeDrawable.SetColor (border.BackgroundColor.ToAndroid ());

				// choose thickest margin
				// the content is padded so it will look like the margin is with the given thickness
				strokeDrawable.SetStroke ((int)context.ToPixels (strokeThickness.ThickestSide()), border.Stroke.ToAndroid ());
				strokeDrawable.SetCornerRadius ((float)border.CornerRadius);
			}

			// create background drawable
			var backgroundDrawable = new GradientDrawable ();

			// set background drawable color based on Border's background color
		    backgroundDrawable.SetColor (border.BackgroundColor.ToAndroid ());
            backgroundDrawable.SetCornerRadius((float)border.CornerRadius);

			if (strokeDrawable != null) {
				// if stroke drawable exists, create a layer drawable containing both stroke and background drawables
				var ld = new LayerDrawable (new Drawable[] { strokeDrawable, backgroundDrawable });
				ld.SetLayerInset (1, (int)context.ToPixels (strokeThickness.Left), (int)context.ToPixels (strokeThickness.Top), (int)context.ToPixels (strokeThickness.Right), (int)context.ToPixels (strokeThickness.Bottom));
				view.SetBackgroundDrawable (ld);
			} else {
				view.SetBackgroundDrawable (backgroundDrawable);
			}

			// set Android.View's padding to take into account the stroke thickiness
			view.SetPadding (
				(int)context.ToPixels (strokeThickness.Left + border.Padding.Left),
				(int)context.ToPixels (strokeThickness.Top + border.Padding.Top),
				(int)context.ToPixels (strokeThickness.Right + border.Padding.Right),
				(int)context.ToPixels (strokeThickness.Bottom + border.Padding.Bottom));
		}

		static double ThickestSide(this Thickness t)
		{
			return new double[] { 
				t.Left, 
				t.Top, 
				t.Right, 
				t.Bottom 
			}.Max();
		}

        public static void SetClipPath(this BorderRenderer br, Canvas canvas)
        {
            var clipPath = new Path();
            //float padding = br;// radius / 2;
            float radius = (float)br.Element.CornerRadius - br.Context.ToPixels((float)br.Element.Padding.ThickestSide());// - padding / 2; // + MaxStrokeThickness());

            int w = (int)br.Width;
            int h = (int)br.Height;
                        
            clipPath.AddRoundRect(new RectF(
                br.ViewGroup.PaddingLeft, 
                br.ViewGroup.PaddingTop,
                w - br.ViewGroup.PaddingRight,
                h - br.ViewGroup.PaddingBottom),
                radius, 
                radius, 
                Path.Direction.Cw);

            canvas.ClipPath(clipPath);
        }
	}
}

