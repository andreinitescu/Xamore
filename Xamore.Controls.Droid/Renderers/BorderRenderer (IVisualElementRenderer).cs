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

[assembly: ExportRendererAttribute(typeof(Border), typeof(BorderRenderer))]

namespace Xamore.Controls.Droid.Renderers
{
	/// <summary>
	///  It's not able to draw the hierarchy of the Border's content
	/// If content is a StackLayout wiht a button, it draws the StackLayout without button
	/// Another issue is that, there are two views appearing instead of just as childs of this renderer's native view:
	/// View (BorderRenderer)
	///     View
	///          View (StackLayout)
	///             
	///     View
	/// </summary>
	public class BorderRenderer : FrameLayout, IVisualElementRenderer
    {
		ViewGroup childView;
        bool isInitialized;
        VisualElementPackager packager;

        #region IVisualElementRenderer implementation

        public VisualElementTracker Tracker { get; private set; }

        public Android.Views.ViewGroup ViewGroup { get { return this; } }

        public VisualElement Element { get; private set; }

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public void SetElement(VisualElement element)
        {
            var oldElement = this.Element;

            //unregister old and re-register new
            if (oldElement != null)
                oldElement.PropertyChanged -= HandlePropertyChanged;

            this.Element = element;
            if (this.Element != null)
            {
                //this.SetBackgroundColor(this.Element.BackgroundColor.ToAndroid());
				BorderRendererVisual.UpdateBackground (BorderElement, this);
				UpdateContent ();

                this.Element.PropertyChanged += HandlePropertyChanged;
            }

            if (!isInitialized)
            {
                isInitialized = true;
                //sizes to match the forms view
                //updates properties, handles visual element properties
                Tracker = new VisualElementTracker(this);
                
				//add and remove children automatically
                packager = new VisualElementPackager(this);
                packager.Load();

                //TODO: this is hardcoded for now via resources, but could be bindable.
                /*SetColorSchemeResources (Resource.Color.xam_dark_blue,
                    Resource.Color.xam_purple,
                    Resource.Color.xam_gray,
                    Resource.Color.xam_green);*/
            }

            if (ElementChanged != null)
            {
                ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
            }
        }

        public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            base.Measure(widthConstraint, heightConstraint);
            return new SizeRequest(new Size((double)base.MeasuredWidth, (double)base.MeasuredHeight), new Size());
        }

        public void UpdateLayout()
        {
            if (Tracker == null)
                return;

            Tracker.UpdateLayout();
        }

        #endregion

		Border BorderElement
		{
			get { return this.Element == null ? null : (Border)Element; }
		}

        public BorderRenderer()
            : base(Forms.Context)
        {
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
				BorderRendererVisual.UpdateBackground (BorderElement, this);
				UpdateContent ();
            }
        }

		private void UpdateContent()
		{
			return;

			ContentView cv;

			if (BorderElement.Content == null)
				return;

			if (childView != null)
				RemoveView (childView);

			var contentRenderer = RendererFactory.GetRenderer (BorderElement.Content);

			childView = contentRenderer.ViewGroup;
			childView.LayoutParameters = new FrameLayout.LayoutParams (FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent);

			AddView (childView);

			// AddView (new Android.Widget.Button (Context) { Text = "AAA" }, new ViewGroup.LayoutParams(300, 300));
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
		}

		protected override void MeasureChild (Android.Views.View child, int parentWidthMeasureSpec, int parentHeightMeasureSpec)
		{
			base.MeasureChild (child, parentWidthMeasureSpec, parentHeightMeasureSpec);
		}

		protected override void MeasureChildren (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.MeasureChildren (widthMeasureSpec, heightMeasureSpec);
		}

		protected override void MeasureChildWithMargins (Android.Views.View child, int parentWidthMeasureSpec, int widthUsed, int parentHeightMeasureSpec, int heightUsed)
		{
			base.MeasureChildWithMargins (child, parentWidthMeasureSpec, widthUsed, parentHeightMeasureSpec, heightUsed);
		}


		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
		}
    }

	public class DefaultRenderer : VisualElementRenderer<ContentView>
	{
	}
}

