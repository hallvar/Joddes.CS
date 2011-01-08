using System;

namespace System.Reflection
{
	public class ConstructorInfo
	{
		public object constructor;
		public ConstructorInfo (object constructor)
		{
			this.constructor = constructor;
		}
		
		public object Invoke (object[] parameters)
		{
		    return Joddes.CS.Html5.Object.create(this.constructor);
		}
	}
}