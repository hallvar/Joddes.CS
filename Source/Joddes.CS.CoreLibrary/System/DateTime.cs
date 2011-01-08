using System;
using Joddes.CS.Html5;

namespace System
{
    public class DateTime
    {
        private Date _date;

        private DateTime (Date date)
        {
            _date = date;
        }

        public DateTime (long ticks)
        {
            _date = new Date(ticks);
        }

        static DateTime Now {
            get {
                return new DateTime (new Date ());
            }
        }

        public static DateTime Parse (string dateTimeStr)
        {
            return new DateTime(new Date(Date.Parse(dateTimeStr)));
        }

        public static TimeSpan operator - (DateTime dateTime1, DateTime dateTime2)
        {
            Date d1 = dateTime1._date;
            Date d2 = dateTime2._date;
            return new TimeSpan(d1.GetTime() - d2.GetTime());
        }

        public static DateTime operator + (DateTime dateTime1, DateTime dateTime2)
        {
            Date d1 = dateTime1._date;
            Date d2 = dateTime2._date;
            return new DateTime (d1.GetTime () + d2.GetTime ());
        }

        public override string ToString ()
        {
            return _date.ToString();
        }
    }
}