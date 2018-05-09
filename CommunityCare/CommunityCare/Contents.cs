using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityCare
{
    class Contents
    {
        // URL of REST service
        public static string RootUrl    = "http://webservice.saudicommunitycare.com/api";
        public static string LoginUrl   = RootUrl + "/client/query?username={0}&password={1}"; //Get
        public static string SignupUrl  = RootUrl + "/client?UserName={0}&Password={1}&Notes={2}&Lat={3}&Long={4}&Gender={5}"; //Post
        public static string ForgotPasswordUrl  = RootUrl + "/client/query?username={0}"; //Post
        public static string ResetPasswordUrl   = RootUrl + "/client/query?UserName={0}&Password={1}&VerifcationCode={2}"; //Post
        public static string GetSurveyUrl       = RootUrl + "/Survey/query?SurveyName={0}"; //Get
        public static string PostAnswersUrl = RootUrl + "/Survey?UserID={0}&SurveyID={1}&SurveyAnswers={2}"; //Post
        public static string GetBloodTestUrl = RootUrl + "/BloodTest?GetBloodTests"; //Get
        public static string CreateOrderBySurveyRecomendation = RootUrl + "/Order/CreateOrderBySurveyRecomendation?" +
                                                                          "UserID={0}&" +
                                                                          "OrderDate={1}%20{2}&" +
                                                                          "Longitude={3}&Latitude={4}&" +
                                                                          "paymentType={5}&" +
                                                                          "Recommendation={6}&" +
                                                                          "SurveyRecomendation={7}"; //POST
        public static string CreateOrderByItems = RootUrl + "/Order/CreateOrderByItems?" +
                                                            "UserID={0}&" +
                                                            "OrderDate={1}%20{2}&" +
                                                            "Longitude={3}&Latitude={4}&" +
                                                            "paymentType={5}&" +
                                                            "Recommendation={6}&" +
                                                            "OrderItems={7}"; //POST
        public static string GetAllClientSurvey = RootUrl + "/Survey/GetAllClientSurvey?UserID={0}";
        public static string GetSurveyAnswer = RootUrl + "/Survey/GetSurveyAnswer?EnrollID={0}";
        public static string GetAllUserOrders = RootUrl + "/Order/GetAllUserOrders?UserID={0}";
        public static string GetAllStaffOrders = RootUrl + "/Order/GetAllStaffOrders?StaffID={0}";
        public static string GetContactInfo = RootUrl + "/Info/GetContactInfo";
        public static string PostContactUS = RootUrl + "/Info/query?UserID={0}&Message={1}";

        public static List<String> paymentTypes = new List<String> { "Cash", "Visa", "Paypal" };
    }
}
