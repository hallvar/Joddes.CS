using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System
{
	[Hidden]
	public class Object
	{
		public Object ()
		{
		}

        [Native("toString")]
        public virtual string ToString ()
        {
            throw new NotSupportedException ();
        }

        public static bool Equals (object objA, object objB)
        {
            return false;
        }

        public virtual int GetHashCode ()
        {
            return 0;
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern Type GetType ();

        public static bool ReferenceEquals (object objA, object objB)
        {
            return (objA == objB);
        }
		/*
		[Native("keys")]
		public static string[] keys (object obj)
		{
			throw new NotSupportedException ();
		}*/
	}
	
	/*
	[Hidden]
	public static class JSObject {
		[Native("__proto__")]
		public static object __proto__ (this Object o)
		{
			throw new NotSupportedException ();
		}
	}*/
}