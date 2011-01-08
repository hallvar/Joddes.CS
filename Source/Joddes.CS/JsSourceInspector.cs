using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;

namespace Joddes.CS
{
    public class JsSourceInspector : JsVisitor
    {
        protected string Namespace { get; set; }
        protected HashSet<string> Usings { get; set; }
        protected ConstructorDeclaration CachedCctor { get; set; }
        protected JsTypeInfo CurrentType { get; set; }
        public List<JsTypeInfo> CollectedTypes { get; protected set; }

        public JsSourceInspector ()
        {
            Usings = new HashSet<string> ();
            CollectedTypes = new List<JsTypeInfo> ();
        }

        public override object VisitCompilationUnit (CompilationUnit node, object data)
        {
            node.AcceptChildren (this, null);
            return null;
        }

        public override object VisitUsingDeclaration (UsingDeclaration node, object data)
        {
            //Usings = new HashSet<string> ();
            foreach (Using item in node.Usings) {
                if (item.IsAlias)
                    throw CreateException (node, "Aliases not are supported");
                
                if (!Usings.Contains (item.Name))
                    Usings.Add (item.Name);
            }
            return null;
        }

        public override object VisitNamespaceDeclaration (NamespaceDeclaration node, object data)
        {
            if (!String.IsNullOrEmpty (Namespace))
                throw CreateException (node, "Nested namespaces are not supported");
            
            string prevNamespace = Namespace;
            //var prevUsings = Usings;
            
            Namespace = node.Name;
            //Usings = new HashSet<string> ();
            
            node.AcceptChildren (this, null);
            
            Namespace = prevNamespace;
            //Usings = prevUsings;
            
            return null;
        }


        public override object VisitTypeDeclaration (TypeDeclaration node, object data)
        {
            if (CurrentType != null) {
                Console.WriteLine ("Nested types are not supported");
                //throw CreateException (node, "Nested types are not supported");
            }
            if (IsHiddenClass (node))
                return null;
            
            if ((node.Modifier & Modifiers.Partial) > 0) {
                Console.WriteLine ("Partial classes are not supported: " + node);
                //throw CreateException (node, "Partial classes are not supported");
            }
            CurrentType = new JsTypeInfo { Name = GenericsHelper.GetScriptName (node), ClassType = node.Type, Namespace = Namespace, Usings = new HashSet<string> (Usings) };
            
            Usings = new HashSet<string> ();
            
            CurrentType.IsStatic = node.Type == ClassType.Enum || HasModifier (node.Modifier, Modifiers.Static);
            
            if (node.Type != ClassType.Interface)
                node.AcceptChildren (this, null);
            
            if (CachedCctor != null) {
                if (!ProcessCctor ())
                    throw CreateException (node, "Static constructor may only contain primitive or array static field initializations");
                CachedCctor = null;
            }
            
            CollectedTypes.Add (CurrentType);
            CurrentType = null;
            
            return null;
        }

        bool IsHiddenClass (TypeDeclaration type)
        {
            foreach (var i in type.Attributes) {
                foreach (var j in i.Attributes) {
                    if (j.Name == "Hidden")
                        return true;
                }
            }
            return false;
        }

        bool HasNativeInlineAttr (MethodDeclaration method)
        {
            foreach (var i in method.Attributes) {
                foreach (var j in i.Attributes) {
                    if (j.Name == "NativeInline")
                        return true;
                }
            }
            return false;
        }

        bool ProcessCctor ()
        {
            foreach (var item in CachedCctor.Body.Children) {
                ExpressionStatement expr = item as ExpressionStatement;
                if (expr == null)
                    return false;
                AssignmentExpression assignment = expr.Expression as AssignmentExpression;
                if (assignment == null)
                    return false;
                if (assignment.Op != AssignmentOperatorType.Assign)
                    return false;
                IdentifierExpression left = assignment.Left as IdentifierExpression;
                if (left == null)
                    return false;
                if (!CurrentType.StaticFields.ContainsKey (left.Identifier))
                    return false;
                if (!IsGoodStaticInitializer (assignment.Right))
                    return false;
                CurrentType.StaticFields[left.Identifier] = new KeyValuePair<Expression, FieldDeclaration> (assignment.Right, null);
            }
            return true;
        }

        static Dictionary<string, object> defaultValues;
        static JsSourceInspector ()
        {
            defaultValues = new Dictionary<string, object> ();
            defaultValues.Add ("Int32", 0);
            defaultValues.Add ("Boolean", false);
            defaultValues.Add ("Double", 0);
        }

        public override object VisitEventDeclaration (EventDeclaration node, object data)
        {
            bool isStatic = CurrentType.ClassType == ClassType.Enum || HasModifier (node.Modifier, Modifiers.Static) || HasModifier (node.Modifier, Modifiers.Const);
            if (isStatic) {
                CurrentType.StaticEvents.Add (node.Name, node);
            } else {
                CurrentType.InstanceEvents.Add (node.Name, node);
            }

            return null;
        }

        public override object VisitFieldDeclaration (FieldDeclaration node, object data)
        {
            bool isStatic = CurrentType.ClassType == ClassType.Enum || HasModifier (node.Modifier, Modifiers.Static) || HasModifier (node.Modifier, Modifiers.Const);
            
            foreach (var item in node.Fields) {
                if (isStatic && !IsGoodStaticInitializer (item.Initializer)) {
                    Console.WriteLine ("Only primitive or array initializers for static fields are supported");
                    //throw CreateException (node, "Only primitive or array initializers for static fields are supported");
                }
                
                Expression initializer = item.Initializer;
                if (initializer.IsNull) {
                    if (CurrentType.ClassType == ClassType.Enum) {
                        Console.WriteLine ("Enum items must be explicitly numbered");
                        //  throw CreateException (node, "Enum items must be explicitly numbered");
                    }
                    initializer = GetDefaultFieldInitializer (node.TypeReference);
                }
                
                if (isStatic) {
                    CurrentType.StaticFields.Add (item.Name, new KeyValuePair<Expression, FieldDeclaration> (initializer, node));
                } else {
                    CurrentType.InstanceFields.Add (item.Name, new KeyValuePair<Expression, FieldDeclaration> (initializer, node));
                }
            }
            return null;
        }
        
        public override object VisitIndexerDeclaration (IndexerDeclaration node, object data)
        {
            
            bool isStatic = CurrentType.ClassType == ClassType.Enum || HasModifier (node.Modifier, Modifiers.Static) || HasModifier (node.Modifier, Modifiers.Const);
            
            if (isStatic) {
                CurrentType.StaticIndexers.Add (node.TypeReference, node);
            } else {
                CurrentType.InstanceIndexers.Add(node.TypeReference, node);
            }
            return null;
        }

        public override object VisitPropertyDeclaration (PropertyDeclaration node, object data)
        {
            bool isStatic = CurrentType.ClassType == ClassType.Enum || HasModifier (node.Modifier, Modifiers.Static) || HasModifier (node.Modifier, Modifiers.Const);
            
            if (isStatic) {
                CurrentType.StaticProperties.Add (node.Name, node);
            } else if (!CurrentType.InstanceProperties.ContainsKey (node.Name)) {
                CurrentType.InstanceProperties.Add (node.Name, node);
            }
            return null;
        }


        Expression GetDefaultFieldInitializer (TypeReference type)
        {
            return new PrimitiveExpression (GetDefaultFieldValue (type), "?");
        }

        object GetDefaultFieldValue (TypeReference type)
        {
            if (type.IsArrayType)
                return null;
            foreach (string key in defaultValues.Keys) {
                if (type.Type == key || type.Type == "System." + key)
                    return defaultValues[key];
            }
            return null;
        }

        bool IsGoodStaticInitializer (Expression expr)
        {
            if (expr.IsNull || expr is PrimitiveExpression)
                return true;
            var arrayExpr = expr as ArrayCreateExpression;
            if (arrayExpr == null)
                return false;
            try {
                new ArrayInitializerVisitor ().VisitArrayCreateExpression (arrayExpr, null);
                return true;
            } catch (ApplicationException) {
                return false;
            }
        }

        class ArrayInitializerVisitor : JsVisitor
        {
            public override object VisitArrayCreateExpression (ArrayCreateExpression node, object data)
            {
                node.ArrayInitializer.CreateExpressions.ForEach (item => item.AcceptVisitor (this, null));
                return null;
            }
            public override object VisitPrimitiveExpression (PrimitiveExpression node, object data)
            {
                return null;
            }
        }

        public override object VisitConstructorDeclaration (ConstructorDeclaration node, object data)
        {
            if (HasModifier (node.Modifier, Modifiers.Static)) {
                CachedCctor = node;
            } else {
                if (CurrentType.Ctor != null) {
                    Console.WriteLine ("Only one constructor is allowed");
                    //throw CreateException (node, "The only constructor is allowed");
                }
                CurrentType.Ctor = node;
            }
            return null;
        }

        public override object VisitMethodDeclaration (MethodDeclaration node, object data)
        {
            if (HasModifier (node.Modifier, Modifiers.Abstract))
                return null;
            if (HasNativeInlineAttr (node))
                return null;
            
            bool isStatic = HasModifier (node.Modifier, Modifiers.Static);
            
            IDictionary<string, MethodDeclaration> dict = isStatic ? CurrentType.StaticMethods : CurrentType.InstanceMethods;
            
            var key = GenericsHelper.GetScriptName (node);
            
            if (dict.ContainsKey (key)) {
                Console.WriteLine ("Overloads are not allowed");
                //throw CreateException (node, "Overloads are not allowed");
            } else {
                dict.Add (key, node);
            }
            return null;
        }


        public override object VisitOperatorDeclaration (OperatorDeclaration node, object data)
        {
            var key = GenericsHelper.GetScriptName (node);

            CurrentType.StaticMethods.Add (key, node);

            return null;
        }

        public static bool HasModifier (Modifiers haystack, Modifiers needle)
        {
            return (haystack & needle) == needle;
        }

        public override object VisitDelegateDeclaration (DelegateDeclaration node, object data)
        {
            if (CurrentType == null)
            {
                return null;
            }
            bool isStatic = HasModifier (node.Modifier, Modifiers.Static);
            IDictionary<string, DelegateDeclaration> dict = isStatic ? CurrentType.StaticDelegates : CurrentType.InstanceDelegates;
            
            var key = GenericsHelper.GetScriptName (node);

            if (dict.ContainsKey (key)) {
                Console.WriteLine ("Overloads are not allowed");
                //throw CreateException (node, "Overloads are not allowed");
            } else {
                dict.Add (key, node);
            }
            return null;
        }
    }
}
