namespace Joddes.CS.Html5
{
	[Hidden, Native("Date")]
	public class Date
	{
		public Date ()
		{
		}

        public Date (long ticks)
        {
        }
		
		[Native("getTime")]
		public long GetTime ()
		{
			throw new System.NotSupportedException();
		}

        [Native("parse")]
        public static long Parse (string dateStr)
        {
            throw new System.NotSupportedException();
        }
	}
}