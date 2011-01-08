using System;

namespace System
{
	[Hidden]
	public class String : Object
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

        [Native("indexOf")]
        public int IndexOf (string str)
        {
            throw new NotImplementedException ();
        }

        [Native("lastIndexOf")]
        public int LastIndexOf (string str)
        {
            throw new NotImplementedException ();
        }

        [Native("substr")]
        public string Substring (int start, int length)
        {
            throw new NotImplementedException();
        }

        [Native("replaceAll")]
        public string Replace (string subStr, string newSubStr)
        {
            throw new NotImplementedException ();
        }
    }
}