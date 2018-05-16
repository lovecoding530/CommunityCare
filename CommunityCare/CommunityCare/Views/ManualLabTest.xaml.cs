using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using ImageButton;
using CommunityCare.Models;
using Plugin.Hud;
using Plugin.Hud.Abstractions;
using CommunityCare.Resx;
using System.Globalization;

namespace CommunityCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManualLabTest : ContentPage
    {
        ObservableCollection<LabTest> labTests = new ObservableCollection<LabTest>();
        ObservableCollection<LabTest> selectedLabTests = new ObservableCollection<LabTest>();

        string locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        public ManualLabTest()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            testsListView.ItemsSource = selectedLabTests;
            testsListView.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };
            CrossHud.Current.Show(message: AppResource.Getting_tests, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var jsonString = await httpClient.GetStringAsync(Contents.GetBloodTestUrl);
            CrossHud.Current.Dismiss();

            Console.WriteLine(jsonString);
            var jsonArray = JArray.Parse(jsonString);

            foreach (var jsonObject in jsonArray)
            {
                var localePre = (locale == "en") ? "e" : "a";
                var testID = jsonObject["btID"].ToString();
                var testName = jsonObject[localePre+"name"].ToString();
                var priceStr = jsonObject["Price"].ToString();
                var price = Convert.ToDouble(priceStr);
                labTests.Add(new LabTest(testID, testName, price));
                labTestPicker.Items.Add(testName);
            }
        }

        async void OnClickedAdd(object sender, EventArgs args)
        {
            if (labTestPicker.SelectedIndex < 0)
            {
                await DisplayAlert(AppResource.CommCare, AppResource.Select_a_Lab_Test, "OK");
                return;
            }
            var item = labTests[labTestPicker.SelectedIndex];
            if (selectedLabTests.Contains(item))
            {
                await DisplayAlert(AppResource.CommCare, AppResource.Aleady_Added, "OK");
            }
            else
            {
                selectedLabTests.Add(item);
            }
            var total = calcTotalPrice();
            totalLabel.Text = "Total: " + total.ToString();
        }

        void OnClickedDelete(object sender, ImageButton.Abstractions.ImageButton.SelectedChangedArgs e)
        {
            var button = sender as ImageButton.Abstractions.ImageButton;
            var item = button.BindingContext as LabTest;
            selectedLabTests.Remove(item);

            var total = calcTotalPrice();
            totalLabel.Text = "Total: " + total.ToString();
        }

        double calcTotalPrice()
        {
            var total = 0.0;
            foreach( var item in selectedLabTests)
            {
                total += item.Price;
            }
            return total;
        }

        async void OnClickedConfirm(object sender, EventArgs args)
        {
            if(selectedLabTests.Count ==  0)
            {
                await DisplayAlert(AppResource.CommCare, AppResource.Please_add_a_lab_test, "OK");
                return;
            }
            var selectedItemsStr = "";
            foreach(var item in selectedLabTests)
            {
                selectedItemsStr += item.ID + ",";
            }

            selectedItemsStr = selectedItemsStr.Remove(selectedItemsStr.Length - 1);
            var paymentTypePage = new PaymentType();
            paymentTypePage.orderItems = selectedItemsStr;
            await Navigation.PushAsync(paymentTypePage);
        }
        async void OnClickedCancel(object sender, EventArgs args)
        {
            var answer = await DisplayAlert(AppResource.CommCare, AppResource.Are_you_sure_to_cancel_the_Lab_Test, "Yes", "No");
            if (answer)
            {
                await Navigation.PopAsync();
            }
        }
    }
}