using System;

namespace System
{
	[Hidden]
	public class String
	{
		public String ()
		{
		}
		
		[Native("length")]
		public int Length {
			get {
				throw new NotImplementedException ();
			}
		}
		
		[Native("{strings}.join({seperator})")]
		public static string Join (string seperator, string[] strings)
		{
			throw new NotImplementedException ();
		}
	}
}
