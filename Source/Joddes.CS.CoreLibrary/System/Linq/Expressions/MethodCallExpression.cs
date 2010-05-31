using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
	public class MethodCallExpression : Expression
	{
		public MethodCallExpression () : base(ExpressionType.Call, typeof(MethodCallExpression))
		{
		
		}
		
		public Expression Object { get; private set; }
		public System.Reflection.MethodInfo Method { get; private set; }
		public ReadOnlyCollection<Expression> Arguments { get; private set; }
	}
}