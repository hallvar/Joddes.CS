using System;
using Joddes.CS.Html5;

namespace System
{
    public class TimeSpan
    {
        long _ticks;

        public TimeSpan (long ticks)
        {
            _ticks = ticks;
        }

        public static TimeSpan operator + (TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            return new TimeSpan (timeSpan1._ticks + timeSpan2._ticks);
        }

        public static TimeSpan operator - (TimeSpan timeSpan1, TimeSpan timeSpan2)
        {
            return new TimeSpan (timeSpan1._ticks - timeSpan2._ticks);
        }

        public double TotalMilliseconds {
            get {
                return _ticks;
            }
        }

        public string ToString ()
        {
            return (string)(object)this._ticks;
        }
    }
}