using System;
namespace System.Linq.Expressions
{
    public class ParameterExpression : Expression
    {
        public ParameterExpression (Type type) : base(ExpressionType.Constant, type)
        {
        }
    }
}