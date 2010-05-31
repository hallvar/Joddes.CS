using System;

namespace Joddes.CS.Html5 {
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