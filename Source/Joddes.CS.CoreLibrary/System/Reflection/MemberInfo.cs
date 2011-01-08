using System;
using System.Collections.Generic;

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

        private delegate object AttributeCreator();

        public object[] GetCustomAttributes (Type type, bool inherit)
        {
            List<object> result = new List<object> ();

            var attributes = (Joddes.CS.Html5.Object[])((Joddes.CS.Html5.Object)((Joddes.CS.Html5.Object)((Joddes.CS.Html5.Object)(object)((ConstructorInfo)(object)ReflectedType.GetConstructors ()[0]).constructor).prototype)[Name])["attributes"];
            if (attributes != null) {
                foreach (Joddes.CS.Html5.Object a in attributes)
                {
                    var body = a.ToString ();
                    //TODO: Make sure only attributes of "type" are added to "result"
                    result.Add (((AttributeCreator)(object)a) ());
                }
            }

            return result.ToArray();
        }
	}
}