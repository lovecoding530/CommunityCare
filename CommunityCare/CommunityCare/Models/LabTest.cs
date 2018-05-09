using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityCare.Models
{
    public class LabTest
    {
        public LabTest(String id, String name, Double price)
        {
            setFields(id, name, price);
        }

        public LabTest(JToken jToken)
        {
            var testID = jToken["btID"].ToString();
            var testName = jToken["ename"].ToString();
            var priceStr = jToken["Price"].ToString();
            var price = Convert.ToDouble(priceStr);
            setFields(testID, testName, price);
        }

        public void setFields(String id, String name, Double price)
        {
            this.ID = id;
            this.Name = name;
            this.Price = price;
        }

        public string ID { private set; get; }

        public string Name { private set; get; }

        public double Price { private set; get; }
    };
}
