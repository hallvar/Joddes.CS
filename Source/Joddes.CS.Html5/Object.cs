namespace Joddes.CS.Html5
{
	[Hidden, Native("Object")]
	public class Object
	{
		public Object ()
		{
		}
		
		[Native("keys")]
		public static string[] keys (object obj)
		{
			throw new System.NotSupportedException ();
		}
		
		[Native("create")]
		public static object create (object constructor)
		{
			throw new System.NotSupportedException ();
		}
		
		[Native("prototype")]
		public object prototype {
			get {
				throw new System.NotSupportedException ();
			}
			set {
				throw new System.NotSupportedException ();
			}
		}

        [Native("getMethodNames")]
        public static string[] GetMethodNames (object obj)
        {
            throw new System.NotSupportedException();
        }

		public object this[string name] {
			get {
				throw new System.NotSupportedException ();
			}
			set {
				throw new System.NotSupportedException ();
			}
		}
	}
}