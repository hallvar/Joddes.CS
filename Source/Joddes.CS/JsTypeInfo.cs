using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Ast;

namespace Joddes.CS
{
    public class JsTypeInfo
    {
        public bool IsStatic { get; set; }
        public ClassType ClassType { get; set; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public HashSet<string> Usings { get; set; }

        // members
        public ConstructorDeclaration Ctor { get; set; }
        public Dictionary<string, KeyValuePair<Expression, FieldDeclaration>> StaticFields { get; protected set; }
        public Dictionary<string, KeyValuePair<Expression, FieldDeclaration>> InstanceFields { get; protected set; }
        public Dictionary<string, PropertyDeclaration> StaticProperties { get; protected set; }
        public Dictionary<string, PropertyDeclaration> InstanceProperties { get; protected set; }
        public Dictionary<string, MethodDeclaration> StaticMethods { get; protected set; }
        public Dictionary<TypeReference, IndexerDeclaration> StaticIndexers { get; protected set; }
        public Dictionary<string, MethodDeclaration> InstanceMethods { get; protected set; }
        public Dictionary<string, DelegateDeclaration> StaticDelegates { get; protected set; }
        public Dictionary<string, DelegateDeclaration> InstanceDelegates { get; protected set; }
        public Dictionary<TypeReference, IndexerDeclaration> InstanceIndexers { get; protected set; }


        public JsTypeInfo ()
        {
            StaticFields = new Dictionary<string, KeyValuePair<Expression, FieldDeclaration>> ();
            InstanceFields = new Dictionary<string, KeyValuePair<Expression, FieldDeclaration>> ();
            StaticProperties = new Dictionary<string, PropertyDeclaration> ();
            InstanceProperties = new Dictionary<string, PropertyDeclaration> ();
            StaticMethods = new Dictionary<string, MethodDeclaration> ();
            InstanceMethods = new Dictionary<string, MethodDeclaration> ();
            StaticDelegates = new Dictionary<string, DelegateDeclaration> ();
            InstanceDelegates = new Dictionary<string, DelegateDeclaration> ();
            InstanceIndexers = new Dictionary<TypeReference, IndexerDeclaration> ();
            StaticIndexers = new Dictionary<TypeReference, IndexerDeclaration>();
        }

        public string FullName {
            get {
                if (String.IsNullOrEmpty (Namespace))
                    return Name;
                return Namespace + "." + Name;
            }
        }
    }
}
