using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CommunityCare.Models
{
    public class Order
    {
        public Order(String orderID, String orderDateTimeStr, Double latitude, Double longitude, int paymentType, String recommendation, List<LabTest> labTests)
        {
            setFields(orderID, orderDateTimeStr, latitude, longitude, paymentType, recommendation, labTests);
        }
        
        public Order(JToken jToken)
        {
            var orderID = jToken["orderID"].ToString();
            var orderDate = jToken["orderDate"].ToString();
            var longitude = jToken["longitude"].ToObject<Double>();
            var latitude = jToken["latitude"].ToObject<Double>();
            var paymentType = jToken["paymentType"].ToObject<int>();
            var recommendation = jToken["recommendation"].ToString();
            var orderItems = jToken["orderItems"] as JArray;
            var labTests = new List<LabTest>();
            foreach (var orderItem in orderItems)
            {
                var bloodTestObject = orderItem["Blood_Test"];
                var bloodTest = new LabTest(bloodTestObject);
                labTests.Add(bloodTest);
            }

            setFields(orderID, orderDate, latitude, longitude, paymentType, recommendation, labTests);
        }

        public void setFields(String orderID, String orderDateTimeStr, Double latitude, Double longitude, int paymentType, String recommendation, List<LabTest> labTests)
        {
            this.orderID = orderID;
            this.orderDateTimeStr = orderDateTimeStr;
            this.orderDateTime = DateTime.Parse(orderDateTimeStr);
            this.orderDateStr = orderDateTime.ToString("MM-dd-yyyy");
            this.latitude = latitude;
            this.longitude = longitude;
            this.paymentType = paymentType;
            this.recommendation = recommendation;
            this.labTests = labTests;
        }

        public string orderID { private set; get; }
        public string orderDateTimeStr { private set; get; }
        public DateTime orderDateTime { private set; get; }
        public string orderDateStr { private set; get; }
        public Double latitude { private set; get; }
        public Double longitude { private set; get; }
        public int paymentType { private set; get; }
        public String recommendation { private set; get; }
        public List<LabTest> labTests { private set; get; }
    };
}
