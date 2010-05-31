using System;

namespace System.Reflection
{
	public class MemberInfo
	{
		public Type DeclaringType { get; set; }
		//public MemberTypes MemberType { get; set; }
		public string Name { get; set; }
		public Type ReflectedType { get; set; }
		//public Module Module { get; set; }
		public int MetadataToken { get; set; }

		public MemberInfo ()
		{
		}
	}
}