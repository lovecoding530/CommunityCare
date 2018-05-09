using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityCare.Models
{
    public class Survey
    {
        public Survey(String enrollID, String dateTimeStr)
        {
            this.enrollID = enrollID;
            this.dateTimeStr = dateTimeStr;
            this.dateTime = DateTime.Parse(dateTimeStr);
            this.dateStr = dateTime.ToString("MM-dd-yyyy");
        }

        public string enrollID { private set; get; }

        public string dateTimeStr { private set; get; }

        public DateTime dateTime { private set; get; }

        public String dateStr { private set; get; }
    };
}
