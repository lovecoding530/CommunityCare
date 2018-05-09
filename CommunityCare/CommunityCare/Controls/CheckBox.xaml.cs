using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CommunityCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class CheckBox : ContentView
    {
        protected string onImg = "checkon.png";
        protected string offImg = "checkoff.png";

        public int Index { get; set; }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                "Text",
                typeof(string),
                typeof(CheckBox),
                null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((CheckBox)bindable).textLabel.Text = (string)newValue;
                });

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                "FontSize",
                typeof(double),
                typeof(CheckBox),
                Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    CheckBox checkbox = (CheckBox)bindable;
                    checkbox.textLabel.FontSize = (double)newValue;
                });

        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create(
                "IsChecked",
                typeof(bool),
                typeof(CheckBox),
                false,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    // Set the graphic.
                    CheckBox checkbox = (CheckBox)bindable;
                    checkbox.box.Source = (bool)newValue ? checkbox.onImg : checkbox.offImg;
                    // Fire the event.
                    EventHandler<bool> eventHandler = checkbox.CheckedChanged;
                    if (eventHandler != null)
                    {
                        eventHandler(checkbox, (bool)newValue);
                    }
                });

        public event EventHandler<bool> CheckedChanged;

        public CheckBox()
        {
            InitializeComponent();
            box.Source = offImg;
        }

        public CheckBox(String onImg, String offImg)
        {
            this.onImg = onImg;
            this.offImg = offImg;
            InitializeComponent();
            box.Source = offImg;
        }

        public string Text
        {
            set { SetValue(TextProperty, value); }
            get { return (string)GetValue(TextProperty); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            set { SetValue(FontSizeProperty, value); }
            get { return (double)GetValue(FontSizeProperty); }
        }

        public bool IsChecked
        {
            set { SetValue(IsCheckedProperty, value); }
            get { return (bool)GetValue(IsCheckedProperty); }
        }

        // TapGestureRecognizer handler.
        void OnCheckBoxTapped(object sender, EventArgs args)
        {
            IsChecked = !IsChecked;
        }
    }
}