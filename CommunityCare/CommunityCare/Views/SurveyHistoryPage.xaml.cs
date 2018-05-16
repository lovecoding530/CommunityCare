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
    public partial class SurveyHistoryPage : ContentPage
    {
        ObservableCollection<Survey> surveys = new ObservableCollection<Survey>();
        string locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        public SurveyHistoryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            listView.ItemsSource = surveys;
            listView.ItemTapped += async (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                var item = listView.SelectedItem as Survey;

                listView.SelectedItem = null; // de-select the row

                CrossHud.Current.Show(message: AppResource.Getting_tests, mask: MaskType.Clear);
                var client = new HttpClient();
                var answersString = await client.GetStringAsync(String.Format(Contents.GetSurveyAnswer, item.enrollID));
                CrossHud.Current.Dismiss();

                var answersArray = JArray.Parse(answersString);

                var questions = new Dictionary<int, JToken>();
                var questionAnswers = new Dictionary<int, List<int>>();

                foreach (var jsonObject in answersArray)
                {
                    var qID = jsonObject["qID"].ToObject<int>();
                    var answerNo = jsonObject["answerNo"].ToObject<int>();
                    var questionObject = jsonObject["Question"];
                    if (!questions.ContainsKey(qID))
                    {
                        questions.Add(qID, questionObject);
                    }
                    if (!questionAnswers.ContainsKey(qID))
                    {
                        questionAnswers.Add(qID, new List<int>());
                    }
                    questionAnswers[qID].Add(answerNo);
                }

                var resultStr = "";
                var qIndex = 0;
                foreach (var question in questions)
                {
                    var qID = question.Key;
                    var questionObject = question.Value;
                    var answers = questionAnswers[qID];
                    var titleKey = (locale == "en") ? "etitle" : "atitle";
                    var title = questionObject[titleKey].ToString();
                    resultStr += String.Format("Q{0}: {1} \n", qIndex + 1, title);

                    var aIndex = 0;
                    foreach (var answerIndex in answers)
                    {
                        var localePre = (locale == "en") ? "e" : "a";
                        var answerKey = "choice" + (answerIndex + 1) + localePre;
                        var answerStr = questionObject[answerKey].ToString();
                        resultStr += String.Format("A{0}: {1} \n", aIndex + 1, answerStr);
                        aIndex++;
                    }
                    resultStr += "\n";
                    qIndex++;
                }
                await DisplayAlert(String.Format("View Survey on {0}", item.dateStr), resultStr, "OK");
            };

            var userId = Application.Current.Properties["clientID"] as String;

            CrossHud.Current.Show(message: AppResource.Getting_tests, mask: MaskType.Clear);
            var httpClient = new HttpClient();
            var jsonString = await httpClient.GetStringAsync(String.Format(Contents.GetAllClientSurvey, userId));
            CrossHud.Current.Dismiss();

            Console.WriteLine(jsonString);
            var jsonArray = JArray.Parse(jsonString);

            foreach (var jsonObject in jsonArray)
            {
                var enrollID = jsonObject["enrollID"].ToString();
                var dateTimeStr = jsonObject["create_date"].ToString();
                surveys.Add(new Survey(enrollID, dateTimeStr));
            }
        }
    }
}