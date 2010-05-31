
using System.Reflection;

namespace System.Linq.Expressions
{
	public class BinaryExpression : Expression
	{
		public Expression Left {
			get; set;
		}

		public MethodInfo Method {
			get; set;
		}

		public Expression Right {
			get; set;
		}

		public BinaryExpression (ExpressionType node_type, Type type)
			: base(node_type, type)
		{
		}
	}
}