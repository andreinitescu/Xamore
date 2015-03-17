using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xamore.Controls.TestApp.Controls
{
    public partial class ControlEditor : ContentView
    {
        public static readonly BindableProperty TargetNameProperty =
            BindableProperty.Create<ControlPropertyEditor, string>(p => p.TargetName, null, BindingMode.OneWay, null, OnTargetNameChanged);

        public string TargetName
        {
            get { return (string)base.GetValue(TargetNameProperty); }
            set { base.SetValue(TargetNameProperty, value); }
        }

        static void OnTargetNameChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var c = (ControlEditor)bindable;
            c.OnTargetNameChanged();
        }

        void OnTargetNameChanged()
        {
            if (!string.IsNullOrWhiteSpace(TargetName))
            {
                PropertyEditor.Target = this.FindByName<VisualElement>(TargetName);
            }
            else
            {
                PropertyEditor.Target = null;

            }
        }

        public ControlEditor()
        {
            InitializeComponent();

            this.BtnShowEditor.GestureRecognizers.Add(new TapGestureRecognizer((v) => 
            { 
                PropertyEditor.IsVisible = !PropertyEditor.IsVisible; 
            }));
        }
    }
}
