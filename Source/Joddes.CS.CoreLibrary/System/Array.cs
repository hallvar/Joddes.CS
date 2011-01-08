namespace System
{
	[Hidden]
	public sealed class Array
	{
		private Array ()
		{
		}

		[Native("length")]
		public int Length {
			get {
				throw new System.NotSupportedException ();
			}
		}

        public int length {
            get {
                throw new System.NotSupportedException();
            }
        }
	}
	
	[Hidden]
	public static class JSArray {
		[Native("join")]
		public static string join (this Array t, string seperator)
		{
			throw new NotSupportedException ();
		}
	}
}