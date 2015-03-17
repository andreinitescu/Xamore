using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace Xamore.Controls.TestApp.Controls
{
    public class ControlPropertyEditor : ListView
    {
        public static readonly BindableProperty TargetProperty =
            BindableProperty.Create<ControlPropertyEditor, VisualElement>(p => p.Target, null, BindingMode.OneWay, null, OnTargetChanged);

        public VisualElement Target
        {
            get { return (VisualElement)base.GetValue(TargetProperty); }
            set { base.SetValue(TargetProperty, value); }
        }

        public static readonly BindableProperty TargetNameProperty =
            BindableProperty.Create<ControlPropertyEditor, string>(p => p.TargetName, null, BindingMode.OneWay, null, OnTargetNameChanged);

        public string TargetName
        {
            get { return (string)base.GetValue(TargetNameProperty); }
            set { base.SetValue(TargetNameProperty, value); }
        }

        static void OnTargetChanged(BindableObject bindable, VisualElement oldValue, VisualElement newValue)
        {
            var c = (ControlPropertyEditor)bindable;
            c.OnTargetChanged();
        }

        static void OnTargetNameChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var c = (ControlPropertyEditor)bindable;
            c.OnTargetNameChanged();
        }

        protected override Cell CreateDefault(object item)
        {
            var pi = (PropertyInfo)item;
            if (pi.PropertyType == typeof(double))
            {
                Binding b = new Binding()
                {
                    Source = Target,
                    Path = pi.Name
                };

                var cell = new EntryCell() { Label = pi.Name };
                cell.SetBinding(EntryCell.TextProperty, b);
                return cell;
                //return CreateSpinnerCell(pi);
            }
            return base.CreateDefault(item);
        }

        Cell CreateThicknessEditorCell(PropertyInfo pi)
        {
            var sl = new StackLayout() { Orientation = StackOrientation.Horizontal };
            Thickness t;
        }

        View CreateThicknessSideEditor(PropertyInfo pi, Thickness t)
        {

        }

        //Cell CreateSpinnerCell(PropertyInfo pi)
        //{
        //    var sl = new StackLayout() { Orientation = StackOrientation.Horizontal };
        //    sl.Children.Add(new Label() { Text = pi.Name });

        //    Binding b = new Binding()
        //    {
        //        Source = Target,
        //        Path = pi.Name
        //    };

        //    var btnInc = new Button() { Text = "+" };
        //    var btnDec = new Button() { Text = "-" };

        //    btnInc.GestureRecognizers.Add(new TapGestureRecognizer((v) => { IncreaseValue(pi, 1); }));
        //    btnDec.GestureRecognizers.Add(new TapGestureRecognizer((v) => { IncreaseValue(pi, -1); }));

        //    sl.Children.Add(btnInc);
        //    sl.Children.Add(btnDec);

        //    var cell = new ViewCell()
        //    {
        //        View = new ContentView()
        //        {
        //            Content = sl
        //        }
        //    };
        //    return cell;
        //}

        //void IncreaseValue(PropertyInfo pi, double step)
        //{
        //    var v = (double)pi.GetValue(Target);
        //    v += step;
        //    pi.SetValue(Target, v);
        //}

        void OnTargetChanged()
        {
            ItemsSource = CreateItems();
        }

        void OnTargetNameChanged()
        {
            if (!string.IsNullOrWhiteSpace(TargetName))
            {
                Target = this.FindByName<VisualElement>(TargetName);
            }
            else
            {
                Target = null;
            }
        }

        IEnumerable CreateItems()
        {
            List<object> items = null;

            if (Target != null)
            {
                items = new List<object>();
                var properties = Target.GetType().GetRuntimeProperties().Where(p => p.CanWrite && p.DeclaringType == Target.GetType()).ToList();
                foreach (PropertyInfo pi in properties)
                {
                    if (pi.PropertyType == typeof(double))
                    {
                        items.Add(pi);
                    }
                }
            }

            return items;
        }

        public class PropertyItem
        {
            public object Value
            {
                get { return PropertyInfo.GetValue(Target); }
                set { PropertyInfo.SetValue(Target, value); }
            }

            public PropertyInfo PropertyInfo { get; set; }

            public VisualElement Target { get; set; }
        }
    }
}
