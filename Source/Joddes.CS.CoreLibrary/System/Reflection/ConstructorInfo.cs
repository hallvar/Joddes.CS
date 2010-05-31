using System;

namespace System.Reflection
{
	public class ConstructorInfo
	{
		object constructor;
		public ConstructorInfo (object constructor)
		{
			this.constructor = constructor;
		}
		
		public object Invoke (object[] parameters)
		{
			return Jsm.Html5.Object.create(this.constructor);
		}
	}
}