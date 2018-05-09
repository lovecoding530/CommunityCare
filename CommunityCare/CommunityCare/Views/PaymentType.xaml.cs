using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CommunityCare
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentType : ContentPage
	{

        public string recommedation;
        public string orderItems;

        public PaymentType ()
		{
            InitializeComponent();

            foreach (string paymentType in Contents.paymentTypes)
            {
                typesPicker.Items.Add(paymentType);
            }
        }

        async void OnClickedConfirm(object sender, EventArgs args)
        {
            if (typesPicker.SelectedIndex < 0)
            {
                await DisplayAlert("ComCare!", "Please select a payment type", "OK");
                return;
            }

            var timeAndLocationPage = new TimeAndLocation();

            timeAndLocationPage.surveyRecommedation = recommedation;
            timeAndLocationPage.orderItems = orderItems;
            timeAndLocationPage.paymentTypeIndex = typesPicker.SelectedIndex;

            await Navigation.PushAsync(timeAndLocationPage);
        }

        async void OnClickedCancel(object sender, EventArgs args)
        {
            var answer = await DisplayAlert("ComCare!", "Are you sure to cancel the Lab Test?", "Yes", "No");
            if (answer)
            {
                await Navigation.PopAsync();
            }
        }
    }
}