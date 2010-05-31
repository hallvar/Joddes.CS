using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
	public class LambdaExpression : Expression
	{
		public Expression Body {
			get; private set;
		}

		public ReadOnlyCollection<ParameterExpression> Parameters {
			get; private set;
		}

		LambdaExpression (Type delegateType, Expression body, ReadOnlyCollection<ParameterExpression> parameters)
			: base (ExpressionType.Lambda, delegateType)
		{
			Body = body;
			Parameters = parameters;
		}
	}
}
