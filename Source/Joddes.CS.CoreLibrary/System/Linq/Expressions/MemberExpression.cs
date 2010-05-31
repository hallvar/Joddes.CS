
using System.Reflection;

namespace System.Linq.Expressions
{
	public class MemberExpression : Expression
	{
		public Expression Expression { get;set; }
		public MemberInfo Member { get; set; }
		
		MemberExpression (Expression expression, MemberInfo member, Type type)
			: base (ExpressionType.MemberAccess, type)
		{
			Expression = expression;
			Member = member;
		}
	}
}