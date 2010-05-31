namespace System.Reflection
{
	public class MethodInfo
	{
		public string Name { get; private set; }

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
	}
}
