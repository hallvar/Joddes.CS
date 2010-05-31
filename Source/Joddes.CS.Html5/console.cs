using System;

namespace Jsm.Html5 {
	[Hidden, Native("console")]
	public class Console
	{
	
		public Console ()
		{
		}
			
		[Native("log")]
		public static void Log (object item)
		{
		} 
	}
}