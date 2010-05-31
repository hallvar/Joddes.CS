using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.Ast;

namespace Joddes.CS
{
    public class LinqExpressionEmitter : JsVisitor
    {
        StringBuilder Output { get; set; }
        public Dictionary<string, object> Locals { get; set; }

        public LinqExpressionEmitter (StringBuilder output)
        {
            Output = output;
        }

        public override object VisitBinaryOperatorExpression (BinaryOperatorExpression binaryOperatorExpression, object data)
        {
            Write ("Object.create(new System.Linq.Expressions.BinaryExpression(), {");
            Write ("Left: { value: ");
            binaryOperatorExpression.Left.AcceptVisitor (this, null);
            Write ("}, Right: { value: ");
            binaryOperatorExpression.Right.AcceptVisitor (this, null);
            Write ("}, Method: { value: 'TODO' }");
            
            Write ("})");
            
            return null;
        }


        public override object VisitQueryExpressionWhereClause (QueryExpressionWhereClause node, object data)
        {
            var parent = (QueryExpression)node.Parent;
            Write ("Object.create(new System.Linq.Expressions.MethodCallExpression(),");
            NewLine ();
            Write ("{ Method: { value: Object.create(new System.Reflection.MethodInfo(),");
            NewLine ();
            Write ("{ Name: { value:'Where'}, Arguments: { value: ['this.Expression(TODO)', ");

            
            NewLine ();
            Write ("Object.create(new System.Linq.Expressions.UnaryExpression(),");
            
            
            NewLine ();
            Write (" { Method: { value:null}, Operand: {value: Object.create(new System.Linq.Expressions.LambdaExpression(),");
            
            
            NewLine ();
            Write (" { Parameters: { value: new System.Collections.ObjectModel.ReadOnlyCollection(new System.Collections.Generic.List()) },");
            NewLine ();
            Write (" Body: { value: ");
            node.Condition.AcceptVisitor (this, null);
            NewLine ();
            Write ("}})}})");
            NewLine();
            Write ("]}})}})");
            //Write (".Where(function(" + parent.FromClause.Identifier + ") { return ");
            //node.Condition.AcceptVisitor (this, null);
            
            return null;
        }

        public override object VisitQueryExpressionSelectClause (QueryExpressionSelectClause node, object data)
        {
            var parent = (QueryExpression)node.Parent;
            Write ("Object.create(new System.Linq.Expressions.MethodCallExpression(), {Method: {value: Object.create(new System.Reflection.MethodInfo(), {Name: {value:'Select'}, Projection: {value: ");
            
            //Write (".Select(function(" + parent.FromClause.Identifier + ") { return ");
            var type = node.Projection.AcceptVisitor (this, null);
            Write ("}})}})");
            
            //Write (";})");
            
            return type;
        }

        public override object VisitIdentifierExpression (IdentifierExpression node, object data)
        {
            //Write ("JDS.initObj(new System.Linq.Expressions.IdentifierExpression(), {Identifier: '"+node.Identifier+"'})");
            Write ("'TODO2'");
            if (Locals.ContainsKey (node.Identifier)) {
                return Locals[node.Identifier];
            }
            
            return null;
        }

        public override object VisitMemberReferenceExpression (MemberReferenceExpression node, object data)
        {
            Write ("Object.create(new System.Linq.Expressions.MemberExpression(), {Member: {value: Object.create(new System.Reflection.MemberInfo(), { Name: {value: '" + node.MemberName + "'}})}, Expression: {value: ");
            node.TargetObject.AcceptVisitor (this, null);
            Write ("}})");
            
            return null;
        }

        public override object VisitPrimitiveExpression (PrimitiveExpression primitiveExpression, object data)
        {
            Write ("Object.create(new System.Linq.Expressions.ConstantExpression(), {Value: {value: '");
            Write (primitiveExpression.Value);
            Write ("'}})");
            
            return null;
            //return primitiveExpression.Value
        }




        /*
        void WriteIndent ()
        {
            if (!IsNewLine)
                return;
            for (var i = 0; i < Level; i++)
                Output.Append ("  ");
            IsNewLine = false;
        }*/
        void NewLine ()
        {
            Output.Append ('\n');
            //IsNewLine = true;
        }

        void Write (object value)
        {
            //WriteIndent ();
            Output.Append (value);
        }

        void Write (params object[] values)
        {
            foreach (var item in values)
                Write (item);
        }
    }
}
