using System.Reflection;

namespace System
{
	public class Type
	{
		object constructor;
		
		public Type (object constructor)
		{
			this.constructor = constructor;
		}
		
		public ConstructorInfo[] GetConstructors ()
		{
			return new ConstructorInfo[1] { new ConstructorInfo(this.constructor) };
		}
		
		public MemberInfo[] GetMembers ()
		{
			//var keys = Object.keys ();
			return null;
		}
		
		public PropertyInfo GetProperty (string name)
		{
            //var type = new Type();
			var pi = new PropertyInfo (name, null);
			return pi;
		}
	}
}