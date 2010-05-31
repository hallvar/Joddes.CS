
using System;

namespace System.Linq.Expressions
{
	public class ConstantExpression : Expression
	{
		public object Value {
			get; private set;
		}
		
		public ConstantExpression (object value, Type type)
			: base(ExpressionType.Constant, type)
		{
			Value = value;
		}
	}
}