namespace System.Reflection
{
	public class MethodInfo : MemberInfo
	{
		public string Name { get; set; }

		public MethodInfo GetBaseDefinition ()
		{
			return null;
		}

		public virtual MethodInfo GetGenericMethodDefinition ()
		{
			return null;
		}

		/*public virtual MethodInfo MakeGenericMethod (params Type[] typeArguments)
		{
			return null;
		}*/

		public virtual Type[] GetGenericArguments ()
		{
			return null;
		}

        public object Invoke (object obj, object[] args)
        {
            return ((Joddes.CS.Html5.Function)((Joddes.CS.Html5.Object)obj)[this.Name]).apply(obj, args);
        }
	}
}
