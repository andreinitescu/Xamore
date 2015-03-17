using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Widget;
using System.Linq;
using Xamore.Controls;
using Xamore.Controls.Droid.Renderers;
using System.ComponentModel;

[assembly: ExportRendererAttribute(typeof(Border), typeof(BorderRenderer))]

namespace Xamore.Controls.Droid.Renderers
{
	// the UI hierarchy resulting is the Border's content is renderer as a View (ContentView's renderer) and it is put as sibling to the LinearLayout
	// View (ContentView's Renderer)
	//   View (ButtonRenderer)
	//     Button
	// LinearLayout
	public class BorderRenderer : ViewRenderer<Border, LinearLayout>
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Border> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement == null)
			{
				base.SetNativeControl(new LinearLayout(base.Context));
			}
			BorderRendererVisual.UpdateBackground (Element, ViewGroup);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			HandlePropertyChanged (sender, e);
		}

		void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
			{
				BorderRendererVisual.UpdateBackground (Element, ViewGroup);
			}
		}
    }
}

