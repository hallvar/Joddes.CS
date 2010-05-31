using System.Reflection;

namespace System.Linq.Expressions
{
	public class UnaryExpression : Expression
	{
		public Expression Operand {
			get; private set;
		}
		
		public MethodInfo Method {
			get; private set;
		}

		public bool IsLifted {
			get; private set;
		}

		//public bool IsLiftedToNull {
		//	get { return is_lifted && this.Type.IsNullable (); }
		//}

		UnaryExpression (ExpressionType nodeType, Expression operand, Type type, MethodInfo method, bool isLifted)
				: base(nodeType, type)
		{
			Operand = operand;
			Method = method;
			IsLifted = isLifted;
		}
	}
}