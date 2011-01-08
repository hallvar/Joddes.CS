using System.Reflection;
using System.Collections.Generic;

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

        public string Name {
            get {
                var name = (string)((Joddes.CS.Html5.Function)this.constructor).name;

                var start = name.LastIndexOf("$");
                return name.Substring(start+1, name.Length-start);
            }
        }

        public string FullName {
            get {
                var name = (string)((Joddes.CS.Html5.Function)this.constructor).name;
                return name.Replace("$", ".");
            }
        }
		
		public MemberInfo[] GetMembers ()
		{
			//var keys = Object.keys ();
			return null;
		}

        public MethodInfo[] GetMethods ()
        {
            List<MethodInfo> methods = new List<MethodInfo> ();
            var names = Joddes.CS.Html5.Object.GetMethodNames (((Joddes.CS.Html5.Object)constructor).prototype);
            foreach (string name in names)
            {
                methods.Add (new System.Reflection.MethodInfo { Name = name, ReflectedType = (Type)(object)this });
            }
            return methods.ToArray();
        }
		
		public PropertyInfo GetProperty (string name)
		{
            //var type = new Type();
			var pi = new PropertyInfo (name, null);
			return pi;
		}

        public MethodInfo GetMethod (string name)
        {
            var names = Joddes.CS.Html5.Object.GetMethodNames (constructor);
            foreach (string methodName in names) {
                if (name == methodName) {
                    return new System.Reflection.MethodInfo { Name = name, ReflectedType = (Type)(object)this };
                }
            }
            return null;
        }
	}
}