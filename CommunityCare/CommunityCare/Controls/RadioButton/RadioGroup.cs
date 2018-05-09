using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CommunityCare
{
    class RadioGroup : StackLayout
    {
        public List<CheckBox> Items;

        public RadioGroup() { 
            Items = new List<CheckBox>();
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(List<String>),
                typeof(RadioGroup),
                null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var radioGroup = bindable as RadioGroup;
                    var newList = newValue as List<String>;
                    radioGroup.Items.Clear();
                    radioGroup.Children.Clear();
                    if (newValue != null)
                    {
                        int radIndex = 0;
                        foreach (var item in newList)
                        {

                            var cb = radioGroup.IsRadio? new RadioButton(): new CheckBox();
                            cb.Text = item.ToString();
                            cb.Index = radIndex;
                            cb.CheckedChanged += radioGroup.OnCheckedChanged;
                            radioGroup.Items.Add(cb);
                            radioGroup.Children.Add(cb);
                            radIndex++;
                        }
                    }
                }
            );

        public static BindableProperty SelectedIndexesProperty =
             BindableProperty.Create(
                 "SelectedIndexes",
                 typeof(List<int>),
                 typeof(RadioGroup),
                 null,
                 propertyChanged: (bindable, oldValue, newValue) =>
                 {
                     var selectedIndexes = newValue as List<int>;
                     if (selectedIndexes.Count == 0) return;

                     var radioGroup = bindable as RadioGroup;

                     if (radioGroup.IsRadio)
                     {
                         var selectedIndex = selectedIndexes[0];
                         foreach (var item in radioGroup.Items)
                         {
                             if (item.Index == selectedIndex)
                             {
                                 item.IsChecked = true;
                             }
                         }
                     }
                     else
                     {
                         foreach(var index in selectedIndexes)
                         {
                             radioGroup.Items[index].IsChecked = true;
                         }
                     }
                 }
             );

        public static readonly BindableProperty IsRadioProperty =
            BindableProperty.Create(
                "IsRadio",
                typeof(bool),
                typeof(RadioGroup),
                false
            );

        public event EventHandler<int> CheckedChanged;

        private void OnCheckedChanged(object sender, bool isChecked)
        {
            var selectedItem = sender as CheckBox;

            if (selectedItem == null) return;

            if (isChecked)
            {
                if (IsRadio)
                {
                    foreach (var item in Items)
                    {
                        if (selectedItem.Index != item.Index)
                        {
                            item.IsChecked = false;
                        }
                        else
                        {
                            SelectedIndexes.Clear();
                            SelectedIndexes.Add(selectedItem.Index);
                            if (CheckedChanged != null)
                            {
                                CheckedChanged.Invoke(sender, selectedItem.Index);
                            }
                        }
                    }
                }
                else
                {
                    SelectedIndexes.Add(selectedItem.Index);
                    if (CheckedChanged != null)
                    {
                        CheckedChanged.Invoke(sender, selectedItem.Index);
                    }
                }
            }
            else
            {
                if (IsRadio)
                {
                    if (SelectedIndexes[0] == selectedItem.Index)
                    {
                        selectedItem.IsChecked = true;
                    }
                }
                else
                {
                    SelectedIndexes.Remove(selectedItem.Index);
                }
            }
        }


        public List<String> ItemsSource
        {
            get { return (List<String>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public List<int> SelectedIndexes
        {
            get { return (List<int>)GetValue(SelectedIndexesProperty); }
            set { SetValue(SelectedIndexesProperty, value); }
        }

        public bool IsRadio
        {
            set { SetValue(IsRadioProperty, value); }
            get { return (bool)GetValue(IsRadioProperty); }
        }

    }
}
