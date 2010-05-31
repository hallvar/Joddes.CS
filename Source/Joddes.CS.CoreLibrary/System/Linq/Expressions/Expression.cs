namespace System.Linq.Expressions {
	public class Expression {
		public ExpressionType NodeType {
			get; private set;
		}

		public Type Type {
			get; private set;
		}

		protected Expression (ExpressionType nodeType, Type type)
		{
			NodeType = nodeType;
			Type = type;
		}
		
		public static ConstantExpression Constant (object expression)
		{
			return new ConstantExpression(expression, expression.GetType());
		}
	}
}