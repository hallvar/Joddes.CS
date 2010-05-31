
using System;

namespace Jsm.Html5
{
	[Hidden]
	public class Date
	{
		public Date ()
		{
		}
		
		[Native("getTime")]
		public long GetTime ()
		{
			return 0;
		}
	}
}