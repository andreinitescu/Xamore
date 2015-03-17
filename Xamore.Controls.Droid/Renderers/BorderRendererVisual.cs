using System;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Linq;

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
				strokeDrawable.SetCornerRadius ((int)context.ToPixels (border.CornerRadius));
			}

			// create background drawable
			var backgroundDrawable = new GradientDrawable ();

			// set background drawable color based on Border's background color
			backgroundDrawable.SetColor (border.BackgroundColor.ToAndroid ());

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

		/* 
		 * if (e.PropertyName == Border.ClipToBoundsProperty.PropertyName)
            {
                OnClipToBoundsChanged();
            }


        void OnClipToBoundsChanged()
        {
            SetWillNotDraw(BorderView.ClipToBounds);
            Invalidate();
        }

		 * protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            SetClipPath(canvas);
        }
        
        void SetClipPath(Canvas canvas)
        {
            return;
            var clipPath = new Path();
            float padding = (float)MaxStrokeThickness();// radius / 2;
            float radius = (float)BorderView.CornerRadius - padding / 2;// + MaxStrokeThickness());

            int w = this.Width;
            int h = this.Height;
            clipPath.AddRoundRect(new RectF(padding, padding, w - padding, h - padding), radius, radius, Path.Direction.Cw);
            //canvas.ClipRect(5, 5, 5, 5);
            canvas.ClipPath(clipPath);
        }
		*/
	}
}

