using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ICSharpCode.NRefactory.Ast;
using Mono.Cecil;
using AstTypeReference = ICSharpCode.NRefactory.Ast.TypeReference;
using CecilTypeReference = Mono.Cecil.TypeReference;
using System.Text.RegularExpressions;

namespace Joddes.CS
{
    public class JsEmitter : JsVisitor
    {
        const string RUNTIME_HELPER_NAME = "JDS";
        const string CREATE_DELEGATE_NAME = "createDelegate";
        const string CAST_NAME = "cast";
        const string TRYCAST_NAME = "tryCast";
        const string IS_NAME = "is";
        const string CREATE_ITERATOR_NAME = "GetEnumerator";
        const string ITERATOR_HAS_NEXT_NAME = "MoveNext";
        const string ITERATOR_NEXT_NAME = "Current";
        const string INIT_OBJECT_NAME = "initObj";

        protected Dictionary<string, object> _locals;
        protected Dictionary<string, object> Locals {
            get { return _locals; }
            set {
                _locals = value;
                LinqExpressionEmitter.Locals = value;
            }
        }
        protected Stack<Dictionary<string, object>> LocalsStack { get; set; }
        protected int Level { get; set; }
        protected bool IsNewLine { get; set; }
        protected bool EnableSemicolon { get; set; }
        protected int IteratorCount { get; set; }
        protected bool UnderscoreIntroduced { get; set; }
        protected int ThisRefCountInMethods { get; set; }
        protected LinqExpressionEmitter LinqExpressionEmitter { get; set; }

        public IDictionary<string, TypeDefinition> KnownTypes { get; protected set; }
        public IDictionary<string, JsTypeInfo> KnownTypeInfos { get; protected set; }
        public JsTypeInfo TypeInfo { get; set; }
        public Mono.Cecil.TypeReference CurrentEventType { get; set; }
        public StringBuilder Output { get; protected set; }

        HashSet<string> _knownNamespaces;
        protected HashSet<string> KnownNamespaces {
            get {
                if (_knownNamespaces == null)
                    _knownNamespaces = CreateKnownNamespaces ();
                return _knownNamespaces;
            }
        }
        HashSet<string> CreateKnownNamespaces ()
        {
            var result = new HashSet<string> ();
            foreach (string typeName in KnownTypes.Keys) {
                int index = typeName.LastIndexOf ('.');
                if (index < 0)
                    continue;
                RegisterNamespace (typeName.Substring (0, index), result);
            }
            return result;
        }

        void RegisterNamespace (string ns, ICollection<string> repository)
        {
            if (String.IsNullOrEmpty (ns)) {
                return;
            }
            
            if (repository.Contains (ns)) {
                return;
            }
            
            string[] parts = ns.Split ('.');
            StringBuilder builder = new StringBuilder ();
            foreach (string part in parts) {
                if (builder.Length > 0)
                    builder.Append ('.');
                builder.Append (part);
                string item = builder.ToString ();
                if (!repository.Contains (item))
                    repository.Add (item);
            }
        }

        protected HashSet<string> EnsuredNamespaces { get; set; }

        public JsEmitter (IDictionary<string, TypeDefinition> knownTypes, List<JsTypeInfo> knownTypeInfos)
        {
            KnownTypeInfos = new Dictionary<string, JsTypeInfo> ();
            KnownTypes = knownTypes;
            
            Output = new StringBuilder ();
            Level = 0;
            IsNewLine = true;
            EnableSemicolon = true;
            
            EnsuredNamespaces = new HashSet<string> ();
            
            foreach (var kti in knownTypeInfos) {
                KnownTypeInfos.Add (GenericsHelper.GetScriptFullName (kti.FullName), kti);
            }
            
            LinqExpressionEmitter = new LinqExpressionEmitter (Output);
        }

        int CompareTypeInfos (JsTypeInfo x, JsTypeInfo y)
        {
            if (x == y)
                return 0;
            if (x.FullName == RUNTIME_HELPER_NAME)
                return -1;
            if (y.FullName == RUNTIME_HELPER_NAME)
                return 1;
            return Comparer.Default.Compare (x.FullName, y.FullName);
        }

        public void Emit (List<JsTypeInfo> types)
        {
            types.Sort (CompareTypeInfos);
            
            foreach (var type in from t in types
                where t.ClassType != ClassType.Interface
                select t) {
                TypeInfo = type;
                
                EmitEnsureNamespace ();
                
                EmitUsings ();
                
                EmitCtor ();
                
                var list = new Dictionary<string, MethodDeclaration>[] { type.StaticMethods, type.InstanceMethods };
                
                foreach (var methodTable in list) {
                    var names = new List<string> (methodTable.Keys);
                    names.Sort ();
                    foreach (var name in names) {
                        VisitMethodDeclaration (methodTable[name], null);
                    }
                }
            }
            
            //EmitEnsureCoreTypes();
        }

        public void Emit (JsTypeInfo type)
        {
            List<string> names;
            TypeInfo = type;
            
            Console.WriteLine ("Emitting {0}", TypeInfo.FullName);
            
            EmitUsings ();
            
            EmitCtor ();
            
            if (TypeInfo.InstanceMethods.Count () > 0 || TypeInfo.InstanceProperties.Count () > 0) {
                
                Write ("_.prototype = {");
                NewLine ();
                Indent ();
                bool first = true;
                
                var types = TypeInfo.InstanceIndexers.Keys;
                //types.Sort ();
                foreach (var t in types) {
                    if (first) {
                        first = false;
                    } else {
                        Write (",");
                        NewLine ();
                        NewLine ();
                    }
                    TypeInfo.InstanceIndexers[t].AcceptVisitor (this, null);
                }
                
                names = new List<string> (TypeInfo.InstanceProperties.Keys);
                names.Sort ();
                foreach (var name in names) {
                    if (first) {
                        first = false;
                    } else {
                        Write (",");
                        NewLine ();
                        NewLine ();
                    }
                    //Write (name, ": ");
                    TypeInfo.InstanceProperties[name].AcceptVisitor (this, null);
                }
                
                names = new List<string> (type.InstanceMethods.Keys);
                foreach (var name in names) {
                    if (first) {
                        first = false;
                    } else {
                        Write (",");
                        NewLine ();
                        NewLine ();
                    }
                    VisitMethodDeclaration (type.InstanceMethods[name], null);
                }
                
                Outdent ();
                NewLine ();
                Write ("};");
                NewLine ();
                NewLine ();
            }
            
            if (type.StaticFields.Count > 0) {
                names = new List<string> (type.StaticFields.Keys);
                foreach (var name in names) {
                    Write ("_.", name, " = ");
                    type.StaticFields[name].Key.AcceptVisitor (this, null);
                    Write (";");
                    NewLine ();
                }
                NewLine ();
            }
            
            if (type.StaticProperties.Count > 0) {
                names = new List<string> (TypeInfo.StaticProperties.Keys);
                names.Sort ();
                foreach (var name in names) {
                    Write ("Object.defineProperty(_, '", name, "', {");
                    NewLine ();
                    Indent ();
                    TypeInfo.StaticProperties[name].AcceptVisitor (this, null);
                    Outdent ();
                    NewLine ();
                    Write ("});");
                    NewLine ();
                }
            }
            
            names = new List<string> (type.StaticMethods.Keys);
            names.Sort ();
            foreach (var name in names) {
                
                VisitMethodDeclaration (type.StaticMethods[name], null);
            }
        }

        public void EmitEnsureNamespace ()
        {
            if (String.IsNullOrEmpty (TypeInfo.Namespace))
                return;
            if (EnsuredNamespaces.Contains (TypeInfo.Namespace))
                return;
            
            RegisterNamespace (TypeInfo.Namespace, EnsuredNamespaces);
            
            var parts = TypeInfo.Namespace.Split ('.');
            
            var check = "";
            
            for (var i = 0; i < parts.Length; i++) {
                if (i == 0) {
                    Write ("var ");
                } else {
                    NewLine ();
                    check += ".";
                }
                check += parts[i];
                Write (check + " = " + check + " || {};");
                
                //Write(parts[i] + ": ("+check+") || { ");
            }
            
            //Write (RUNTIME_HELPER_NAME, ".", ENSURE_NAMESPACE_NAME);
            //Write ("(");
            //WriteScript (TypeInfo.Namespace);
            //Write (");");
            NewLine ();
            NewLine ();
        }

        public void EmitUsings ()
        {
            if (!TypeInfo.Usings.Contains (TypeInfo.Namespace)) {
                TypeInfo.Usings.Add (TypeInfo.Namespace);
            }
            
            bool first = true;
            foreach (TypeDefinition t in (from k in KnownTypes
                where (from u2 in TypeInfo.Usings
                    where k.Key.StartsWith (u2)
                    select u2).Count () > 0 && k.Key != TypeInfo.FullName && !k.Value.IsInterface
                select k.Value).Distinct ()) {
                var hidden = GetAttribute ("Hidden", t.CustomAttributes) != null;
                if (!hidden && !t.FullName.Contains ("/")) {
                    if (!first) {
                        Write (",\n          ");
                    } else {
                        Write ("JDS.using(");
                        first = false;
                    }
                    Write ("'" + GenericsHelper.GetScriptFullName (t) + "'");
                }
            }
            
            if (!first) {
                Write (");");
                NewLine ();
                NewLine ();
            }
        }

        public void EmitCtor ()
        {
            var requireUnderscore = TypeInfo.StaticMethods.Count > 0 || TypeInfo.InstanceMethods.Count > 0 || !TypeInfo.IsStatic && TypeInfo.StaticFields.Count > 0;
            
            //if (requireUnderscore) {
            //if (!UnderscoreIntroduced) {
            //      Write ("var ");
            //      UnderscoreIntroduced = true;
            //  }
            //  Write ("_ = ");
            //}
            
            //Write(TypeInfo.FullName, " = ");
            switch (TypeInfo.ClassType) {
            case ClassType.Class:
                if (TypeInfo.IsStatic)
                    EmitCtorForStaticClass ();
                else
                    EmitCtorForInstantiableClass ();
                break;
            case ClassType.Enum:
                EmitCtorForEnum ();
                break;
            case ClassType.Interface:
                Write ("{ };");
                NewLine ();
                break;
            default:
                throw new ApplicationException (string.Format ("Unsupported class type: {0}", TypeInfo.ClassType));
            }
            
            NewLine ();
        }

        void EmitCtorForInstantiableClass ()
        {
            var ctor = TypeInfo.Ctor ?? new ConstructorDeclaration ("", Modifiers.Public, new List<ParameterDeclarationExpression> (), new List<AttributeSection> ());
            var baseType = GetBaseTypeDefinition ();
            ResetLocals ();
            AddLocals (ctor.Parameters);
            
            Write ("var _ = JDS.defineClass(function ", TypeInfo.FullName.Replace (".", "$"));
            
            EmitMethodParameters (ctor.Parameters, ctor);
            Write (" ");
            BeginBlock ();
            
            bool first = true;
            
            var names = new List<string> (TypeInfo.InstanceFields.Keys);
            foreach (var name in names) {
                Write ("this.", name, " = ");
                TypeInfo.InstanceFields[name].Key.AcceptVisitor (this, null);
                Write (";");
                NewLine ();
            }
            if (names.Count () > 0) {
                NewLine ();
            }
            
            var requireNewLine = false;
            
            // base ctor call
            if (baseType != null && !JsMetadataChecker.HasHiddenAttribute (baseType)) {
                if (requireNewLine)
                    NewLine ();
                var initializer = ctor.ConstructorInitializer ?? new ConstructorInitializer { ConstructorInitializerType = ConstructorInitializerType.Base };
                if (initializer.ConstructorInitializerType == ConstructorInitializerType.This)
                    throw CreateException (ctor, "Multiple ctors are not supported");
                Write (GenericsHelper.GetScriptFullName (baseType));
                Write (".call(this");
                foreach (var p in initializer.Arguments) {
                    WriteComma ();
                    p.AcceptVisitor (this, null);
                }
                Write (");");
                NewLine ();
                requireNewLine = true;
            }
            
            // body                
            var nativeImpl = GetNativeImplLines (ctor);
            if (nativeImpl == null) {
                if (ctor.Body.Children.Count > 0) {
                    if (requireNewLine)
                        NewLine ();
                    ctor.Body.Children.ForEach (i => i.AcceptVisitor (this, null));
                }
            } else {
                if (requireNewLine)
                    NewLine ();
                foreach (var line in nativeImpl) {
                    Write (line);
                    NewLine ();
                }
            }
            
            EndBlock ();
            Write (");");
            NewLine ();
        }

        void EmitCtorForStaticClass ()
        {
            Write ("JDS.setUsingsLoaded('", TypeInfo.FullName, "');");
            NewLine ();
            var sortedNames = new List<string> (TypeInfo.StaticFields.Keys);
            sortedNames.Sort ();
            
            BeginBlock ();
            for (var i = 0; i < sortedNames.Count; i++) {
                var name = sortedNames[i];
                Write (name, ": ");
                TypeInfo.StaticFields[name].Key.AcceptVisitor (this, null);
                if (i < sortedNames.Count - 1)
                    Write (",");
                NewLine ();
            }
            EndBlock ();
            Write (";");
            NewLine ();
        }

        void EmitCtorForEnum ()
        {
            Write ("var _ = JDS.defineClass(function ", TypeInfo.FullName.Replace (".", "$"), "() {});");
            NewLine ();
        }

        public void EmitTypeRegistration ()
        {
            if (TypeInfo == null) {
                Console.WriteLine ("TypeInfo is null, fix");
            }
            if (TypeInfo.IsStatic)
                return;
            Write ("_");
            Write ("(");
            WriteScript (TypeInfo.FullName);
            WriteComma ();
            Write ("[");
            
            var list = new List<string> ();
            var baseType = GetBaseTypeDefinition ();
            if (baseType != null)
                list.Add (GenericsHelper.GetScriptFullName (baseType));
            
            var type = GetTypeDefinition ();
            if (type != null) {
                foreach (CecilTypeReference i in type.Interfaces) {
                    if (i.FullName == "System.Collections.IEnumerable")
                        continue;
                    if (i.FullName == "System.Collections.IEnumerator")
                        continue;
                    list.Add (GenericsHelper.GetScriptFullName (i));
                }
            }
            
            foreach (var item in list) {
                if (item != list[0])
                    WriteComma ();
                Write (ShortenTypeName (item));
            }
            
            Write ("]);");
            NewLine ();
        }

        void EmitLambda (List<ParameterDeclarationExpression> parameters, INode body, INode context, Mono.Collections.Generic.Collection<ParameterDefinition> paramdefinitions)
        {
            PushLocals ();
            if (paramdefinitions == null) {
                AddLocals (parameters);
            } else {
                foreach (ParameterDefinition p in paramdefinitions) {
                    Locals.Add (p.Name, p.ParameterType);
                }
            }
            
            bool block = body is BlockStatement;
            
            Write ("");
            var savedPos = Output.Length;
            var savedThisCount = ThisRefCountInMethods;
            
            Write ("function");
            EmitMethodParameters (parameters, context);
            Write (" ");
            if (!block) {
                Write ("{ ");
            }
            if (body.Parent is LambdaExpression && !block)
                Write ("return ");
            body.AcceptVisitor (this, null);
            if (!block) {
                Write (" }");
            }
            
            if (ThisRefCountInMethods > savedThisCount) {
                //Output.Insert(savedPos, RUNTIME_HELPER_NAME + ".bind(this)(");
                Write (".bind(this)");
            }
            
            PopLocals ();
        }

        void EmitBlockOrIndentedLine (INode node)
        {
            bool block = node is BlockStatement;
            if (!block) {
                NewLine ();
                Indent ();
            } else {
                Write (" ");
            }
            node.AcceptVisitor (this, null);
            if (!block)
                Outdent ();
        }

        void EmitMethodParameters (IList<ParameterDeclarationExpression> list, INode context)
        {
            Write ("(");
            foreach (var p in list) {
                CheckIdentifier (p.ParameterName, context);
                if (p != list[0])
                    WriteComma ();
                Write (p.ParameterName);
            }
            Write (")");
        }

        void EmitExpressionList (IList<Expression> list)
        {
            foreach (var a in list) {
                if (a != list[0])
                    WriteComma ();
                a.AcceptVisitor (this, null);
            }
        }

        // Writer

        void Indent ()
        {
            ++Level;
        }

        void Outdent ()
        {
            if (Level > 0)
                Level--;
        }

        void WriteIndent ()
        {
            if (!IsNewLine)
                return;
            for (var i = 0; i < Level; i++)
                Output.Append ("  ");
            IsNewLine = false;
        }

        void NewLine ()
        {
            Output.Append ('\n');
            IsNewLine = true;
        }

        void BeginBlock ()
        {
            Write ("{");
            NewLine ();
            Indent ();
        }

        void EndBlock ()
        {
            Outdent ();
            Write ("}");
        }

        void Write (object value)
        {
            WriteIndent ();
            Output.Append (value);
        }

        void Write (params object[] values)
        {
            foreach (var item in values)
                Write (item);
        }

        void WriteScript (object value)
        {
            WriteIndent ();
            Output.Append (ToJavaScript (value));
        }

        //void WriteComment(string text) {
        //    Write("// " + text);
        //    NewLine();
        //}

        void WriteComma ()
        {
            WriteComma (false);
        }


        void WriteComma (bool newLine)
        {
            Write (",");
            if (newLine)
                NewLine ();
            else
                Write (" ");
        }

        void WriteThis ()
        {
            Write ("this");
            ThisRefCountInMethods++;
        }

        void WriteObjInitializer (List<Expression> expressions)
        {
            foreach (NamedArgumentExpression item in expressions) {
                if (item != expressions[0])
                    WriteComma ();
                Write (item.Name, ": ");
                item.Expression.AcceptVisitor (this, null);
            }
        }

        string ToJavaScript (object value)
        {
            if (value == null)
                return "null";
            if (value is Boolean)
                return (Boolean)value ? "true" : "false";
            if (value is Int32 || value is Double)
                return String.Format (System.Globalization.CultureInfo.InvariantCulture, "{0}", value);
            if (value is String) {
                var builder = new StringBuilder ();
                foreach (var ch in value.ToString ()) {
                    switch (ch) {
                    case '\\':
                    case '\'':
                        builder.Append ('\\');
                        builder.Append (ch);
                        break;
                    case '\n':
                        builder.Append ("\\n");
                        break;
                    case '\r':
                        builder.Append ("\\r");
                        break;
                    case '\t':
                        builder.Append ("\\t");
                        break;
                    case '\0':
                        builder.Append ("\\0");
                        break;
                    default:
                        builder.Append (ch);
                        break;
                    }
                }
                return "'" + builder.ToString () + "'";
            }
            throw new ApplicationException (string.Format ("Cannot convert value of type {0} to javascript", value.GetType ().FullName));
        }

        bool KeepLineAfterBlock (BlockStatement block)
        {
            var parent = block.Parent;
            if (parent is AnonymousMethodExpression)
                return true;
            if (parent is LambdaExpression)
                return true;
            if (parent is MethodDeclaration)
                return true;
            if (parent is PropertySetRegion)
                return true;
            if (parent is PropertyGetRegion)
                return true;
            var loop = parent as DoLoopStatement;
            if (loop != null && loop.ConditionType == ConditionType.While && loop.ConditionPosition == ConditionPosition.End)
                return true;
            return false;
        }

        // Ids and tokens

        static HashSet<string> InvalidIdentifiers = new HashSet<string> (new[] { "_", "arguments", "boolean", "debugger", "delete", "export", "extends", "final", "function", "implements",
        "import", "instanceof", "native", "package", "super", "synchronized", "throws", "transient", "var", "with" });

        void CheckIdentifier (string name, INode context)
        {
            if (InvalidIdentifiers.Contains (name)) {
                Console.WriteLine ("ERROR: Cannot use '" + name + "' as identifier");
                //throw CreateException (context, "Cannot use '" + name + "' as identifier");
            }
        }

        string GetNextIteratorName ()
        {
            var index = IteratorCount++;
            var result = "$i";
            if (index > 0)
                result += index;
            return result;
        }

        // Symbol resolution

        IMemberDefinition ResolveFieldOrMethod (string name, int genericCount)
        {
            bool allowPrivate = true;
            TypeDefinition type = GetTypeDefinition ();
            TypeDefinition thisType = type;
            while (true) {
                if (type == null) {
                    Console.WriteLine ("ResolveFieldOrMethod, type is null");
                    return null;
                }
                foreach (MethodDefinition method in type.Methods) {
                    if (method.Name != name || method.GenericParameters.Count != genericCount)
                        continue;
                    if (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly)
                        return method;
                    if (method.IsPrivate && allowPrivate)
                        return method;
                    if (method.IsAssembly && type.Module.Mvid == thisType.Module.Mvid) {
                        return method;
                    }
                }
                foreach (FieldDefinition field in type.Fields) {
                    if (field.Name != name)
                        continue;
                    if (field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly)
                        return field;
                    if (field.IsPrivate && allowPrivate)
                        return field;
                    if (field.IsAssembly && type.Module.Mvid == thisType.Module.Mvid) {
                        return field;
                    }
                }
                type = GetBaseTypeDefinition (type);
                if (type == null)
                    break;
                allowPrivate = false;
            }
            return null;
        }

        string ResolveNamespaceOrType (string id, bool allowNamespaces)
        {
            if (allowNamespaces && KnownNamespaces.Contains (id))
                return id;
            if (KnownTypes.ContainsKey (id))
                return id;
            
            string guess;
            
            string namespacePrefix = TypeInfo.Namespace;
            if (!String.IsNullOrEmpty (namespacePrefix)) {
                while (true) {
                    guess = namespacePrefix + "." + id;
                    if (allowNamespaces) {
                        if (KnownNamespaces.Contains (guess))
                            return guess;
                    }
                    
                    //Console.WriteLine (guess);
                    if (KnownTypes.ContainsKey (guess))
                        return guess;
                    
                    
                    
                    
                    
                    
                    
                    
                    int index = namespacePrefix.LastIndexOf (".");
                    if (index < 0)
                        break;
                    namespacePrefix = namespacePrefix.Substring (0, index);
                }
            }
            
            if (Locals.ContainsKey (id)) {
                var tr = Locals[id] as ICSharpCode.NRefactory.Ast.TypeReference;
                
                if (tr != null) {
                    return ResolveType (tr.Type);
                } else {
                    var tr2 = Locals[id] as Mono.Cecil.ParameterDefinition;
                    if (tr2 != null) {
                        return ResolveType (tr2.ParameterType.Name);
                    }
                }
            }
            
            foreach (string usingPrefix in TypeInfo.Usings) {
                guess = usingPrefix + "." + id;
                
                if (KnownTypes.ContainsKey (guess))
                    return guess;
            }
            if (TypeInfo.InstanceFields.ContainsKey (id)) {
                return ResolveType (TypeInfo.InstanceFields[id].Value.TypeReference.Type);
            }
            
            if (TypeInfo.StaticFields.ContainsKey (id)) {
                return ResolveType (TypeInfo.StaticFields[id].Value.TypeReference.Type);
            }
            
            if (TypeInfo.InstanceMethods.ContainsKey (id)) {
                return null;
            }
            
            if (id.IndexOf ("[]") > 0) {
                return id;
            }
            
            Console.WriteLine ("No Type: {0}", id);
            if (id == "ci") {
                throw new Exception ();
            }
            
            if (id == "IEnumerable`1") {
                throw new Exception ();
            }
            return null;
        }

        string ResolveType (string id)
        {
            return ResolveNamespaceOrType (id, false);
        }

        TypeDefinition GetTypeDefinition ()
        {
            if (KnownTypes.ContainsKey (GenericsHelper.GetTypeMapKey (TypeInfo))) {
                return KnownTypes[GenericsHelper.GetTypeMapKey (TypeInfo)];
            }
            Console.WriteLine ("KnownTypes: " + GenericsHelper.GetTypeMapKey (TypeInfo) + " not in list.");
            return null;
        }

        TypeDefinition GetTypeDefinition (AstTypeReference reference)
        {
            string name = GenericsHelper.GetScriptName (reference);
            name = ResolveType (name);
            if (name != null) {
                return KnownTypes[name];
            }
            Console.WriteLine ("Couldn't get TypeDefinition: " + name);
            return null;
        }

        TypeDefinition GetBaseTypeDefinition ()
        {
            return GetBaseTypeDefinition (GetTypeDefinition ());
        }

        TypeDefinition GetBaseTypeDefinition (TypeDefinition type)
        {
            if (KnownTypes.ContainsKey (GenericsHelper.GetTypeMapKey (type))) {
                var reference = KnownTypes[GenericsHelper.GetTypeMapKey (type)].BaseType;
                if (reference == null)
                    return null;
                if (KnownTypes.ContainsKey (GenericsHelper.GetTypeMapKey (reference))) {
                    return KnownTypes[GenericsHelper.GetTypeMapKey (reference)];
                } else {
                    Console.WriteLine ("GetBaseTypeDefinition (reference): " + GenericsHelper.GetTypeMapKey (reference) + " not in KnownTypes.");
                }
            } else {
                Console.WriteLine ("GetBaseTypeDefinition: " + GenericsHelper.GetTypeMapKey (type) + " not in KnownTypes.");
            }
            return null;
        }

        TypeDefinition GetBaseMethodOwnerTypeDefinition (string methodName, int genericParamCount)
        {
            TypeDefinition type = GetBaseTypeDefinition ();
            while (true) {
                if (type == null) {
                    Console.WriteLine ("GetBaseMethodOWnerTypeDefinition: type is null");
                    return null;
                }
                foreach (var method in from m in type.Methods
                    where m.Name == methodName
                    select m) {
                    if (genericParamCount < 1 || method.GenericParameters.Count == genericParamCount)
                        return type;
                }
                type = GetBaseTypeDefinition (type);
            }
        }

        string ShortenTypeName (string name)
        {
            var type = KnownTypes[name];
            var customName = JsMetadataChecker.GetCustomTypeName (type);
            if (!String.IsNullOrEmpty (customName)) {
                if (!JsMetadataChecker.HasHiddenAttribute (type))
                    throw new ApplicationException (string.Format ("Custom type names are only allowed for Hidden types: {0}", type));
                return customName;
            }
            return name;
        }

        // Attributes

        ICSharpCode.NRefactory.Ast.Attribute GetMethodAttribute (AttributedNode method, string name)
        {
            foreach (var i in method.Attributes) {
                foreach (var j in i.Attributes) {
                    if (j.Name == name)
                        return j;
                }
            }
            return null;
        }

        ICSharpCode.NRefactory.Ast.Attribute GetPropertyAttribute (AttributedNode method, string name)
        {
            foreach (var i in method.Attributes) {
                foreach (var j in i.Attributes) {
                    if (j.Name == name)
                        return j;
                }
            }
            return null;
        }

        bool HasDelegateAttribute (MethodDeclaration method)
        {
            return GetMethodAttribute (method, "Delegate") != null;
        }

        string GetNativeInlineImpl (InvocationExpression node)
        {
            var parts = new List<string> ();
            Expression current = node.TargetObject;
            var genericCount = -1;
            
            
            while (true) {
                //Console.WriteLine ("Current: " + current);
                
                MemberReferenceExpression member = current as MemberReferenceExpression;
                if (member != null) {
                    parts.Insert (0, member.MemberName);
                    current = member.TargetObject;
                    if (genericCount < 0)
                        genericCount = member.TypeArguments.Count;
                    continue;
                }
                IdentifierExpression id = current as IdentifierExpression;
                if (id != null) {
                    parts.Insert (0, id.Identifier);
                    if (genericCount < 0)
                        genericCount = id.TypeArguments.Count;
                    break;
                }
                TypeReferenceExpression typeRef = current as TypeReferenceExpression;
                if (typeRef != null) {
                    parts.Insert (0, GenericsHelper.GetScriptName (typeRef.TypeReference));
                    break;
                }
                break;
            }
            
            if (parts.Count < 1)
                return null;
            if (genericCount < 0)
                genericCount = 0;
            
            var name = String.Join (".", parts.ToArray (), 0, parts.Count - 1);
            
            string typeName = TypeInfo.FullName;
            if (Locals.ContainsKey (name)) {
                typeName = Locals[name].ToString ();
            } else if (parts.Count >= 2) {
                typeName = ResolveType (name);
            }
            
            //Console.WriteLine ("TypeName: " + String.Join(".", parts.ToArray()));
            
            if (String.IsNullOrEmpty (typeName))
                return null;
            
            
            string methodName = parts[parts.Count - 1];
            
            if (KnownTypes.ContainsKey (typeName)) {
                TypeDefinition type = KnownTypes[typeName];
                MethodDefinition[] methods = (from m in type.Methods
                    where m.Name == methodName
                    select m).ToArray ();
                
                foreach (var method in methods) {
                    if (method.IsStatic && method.Parameters.Count == node.Arguments.Count && method.GenericParameters.Count == genericCount)
                        return JsMetadataChecker.GetNativeInlineImpl (method);
                }
            }
            
            return null;
        }

        IEnumerable<string> GetNativeImplLines (ParametrizedNode method)
        {
            var attr = GetMethodAttribute (method, "Native");
            if (attr == null)
                return null;
            var result = new List<string> ();
            foreach (var arg in attr.PositionalArguments) {
                PrimitiveExpression expr = (PrimitiveExpression)arg;
                result.Add ((string)expr.Value);
            }
            return result;
        }

        string GetNativeName (Mono.Collections.Generic.Collection<CustomAttribute> attributes)
        {
            foreach (CustomAttribute a in attributes) {
                //Console.WriteLine ("=== Name: {0}", a.Constructor.DeclaringType.Name);
                if (a.Constructor.DeclaringType.Name == "NativeAttribute") {
                    //Console.WriteLine ("**** Parameter: {0}", a.ConstructorParameters[0][0]);
                    return (string)((Mono.Cecil.CustomAttributeArgument[])a.ConstructorArguments[0].Value)[0].Value;
                }
            }
            return null;
            //return (from a in attributes
            //  where a.Constructor.Name == "Native"
            //  select (string)a.ConstructorParameters[0]).FirstOrDefault ();
        }

        public bool IsEvent (Expression node)
        {
            var reference = node as MemberReferenceExpression;
            if (reference != null) {
                var type = ResolveOwnerType (reference);
                
                var tr = type as TypeDefinition;
                if (tr != null) {
                    
                    var e = FindEvent (reference.MemberName, tr);
                    if (e != null) {
                        return true;
                    }
                }
            }
            
            return false;
        }

        public string GetEventName (Expression node)
        {
            var reference = node as MemberReferenceExpression;
            if (reference != null) {
                var type = ResolveOwnerType (reference);
                var tr = type as TypeDefinition;
                if (tr != null) {
                    var e = FindEvent (reference.MemberName, tr);
                    if (e != null) {
                        var attr = GetAttribute ("Native", e.CustomAttributes);
                        if (attr != null) {
                            var lines = (Mono.Cecil.CustomAttributeArgument[])attr.ConstructorArguments[0].Value;
                            return (string)lines[0].Value;
                            
                            //Write(String.Join ("\n", lines));
                        } else {
                            return reference.MemberName;
                        }
                    }
                }
            }
            
            return null;
        }

        public EventDefinition FindEvent (string eventName, TypeDefinition type)
        {
            foreach (EventDefinition e in type.Events) {
                if (e.Name == eventName) {
                    return e;
                }
            }
            return null;
        }

        // Local vars

        void PushLocals ()
        {
            if (LocalsStack == null)
                LocalsStack = new Stack<Dictionary<string, object>> ();
            LocalsStack.Push (Locals);
            Locals = new Dictionary<string, object> (Locals);
        }

        void PopLocals ()
        {
            Locals = LocalsStack.Pop ();
        }

        void ResetLocals ()
        {
            Locals = new Dictionary<string, object> ();
            IteratorCount = 0;
        }

        void AddLocals (List<ParameterDeclarationExpression> list)
        {
            list.ForEach (item => Locals.Add (item.ParameterName, item.TypeReference));
        }

        // Visitor members

        public override object VisitMethodDeclaration (MethodDeclaration node, object data)
        {
            ResetLocals ();
            AddLocals (node.Parameters);
            
            bool isStatic = JsSourceInspector.HasModifier (node.Modifier, Modifiers.Static);
            
            if (node.IsExtensionMethod) {
                
                Write ("window.addEventListener('usingsloaded', function() ");
                //NewLine ();
                BeginBlock ();
                foreach (TypeDefinition t in KnownTypes.Values) {
                    if (t.IsInterface) {
                        continue;
                    }
                    bool hasThisInterface = false;
                    var name = ResolveType (node.Parameters[0].TypeReference.Type);
                    foreach (Mono.Cecil.TypeReference i in t.Interfaces) {
                        //Console.WriteLine ("Path: {0} == {1}", i.FullName, name);
                        if (GenericsHelper.GetScriptFullName (i) == name) {
                            hasThisInterface = true;
                            break;
                        }
                    }
                    
                    if (hasThisInterface) {
                        Write (GenericsHelper.GetScriptFullName (t), ".prototype.", GenericsHelper.GetScriptName (node), " = ");
                    }
                }
            } else {
                if (isStatic) {
                    Write ("_.", GenericsHelper.GetScriptName (node), " = ");
                } else {
                    Write (GenericsHelper.GetScriptName (node));
                    Write (": ");
                }
            }
            Write ("function");
            EmitMethodParameters (node.Parameters, node);
            Write (" ");
            
            
            var nativeImpl = GetNativeImplLines (node);
            if (nativeImpl == null) {
                node.Body.AcceptVisitor (this, null);
                if (isStatic) {
                    Write (";");
                }
            } else {
                BeginBlock ();
                foreach (var line in nativeImpl) {
                    Write (line);
                    NewLine ();
                }
                EndBlock ();
                if (isStatic) {
                    Write (";");
                    NewLine ();
                }
            }
            if (node.IsExtensionMethod) {
                EndBlock ();
                Write (");");
            }
            
            return null;
        }

        public override object VisitBlockStatement (BlockStatement node, object data)
        {
            PushLocals ();
            
            BeginBlock ();
            node.Children.ForEach (child => child.AcceptVisitor (this, null));
            EndBlock ();
            
            if (!KeepLineAfterBlock (node))
                NewLine ();
            
            PopLocals ();
            return null;
        }

        public override object VisitLocalVariableDeclaration (LocalVariableDeclaration node, object data)
        {
            var type = (object)node.TypeReference;
            type = node.TypeReference.Type == "var" ? null : type;
            Write ("var ");
            foreach (var variable in node.Variables) {
                CheckIdentifier (variable.Name, node);
                Locals.Add (variable.Name, null);
                
                if (variable != node.Variables[0])
                    WriteComma ();
                
                Write (variable.Name);
                if (!variable.Initializer.IsNull) {
                    Write (" = ");
                    var returnType = variable.Initializer.AcceptVisitor (this, null);
                    if (type == null) {
                        type = returnType;
                    }
                    //Console.WriteLine ("Initializer: " + variable.Initializer);
                    
                    //Console.WriteLine ("{0} Return Type: {1}", variable.Name, type);
                    if (type != null) {
                        Locals[variable.Name] = type;
                        //Console.WriteLine("ReturnType: "+returnType);
                    } else {
                        Console.WriteLine ("Initializer: " + variable.Initializer);
                        throw new Exception ();
                    }
                }
            }
            if (EnableSemicolon) {
                Write (";");
                NewLine ();
            }
            return type;
        }

        public override object VisitPrimitiveExpression (PrimitiveExpression node, object data)
        {
            if (node.IsNull)
                return null;
            WriteScript (node.Value);
            return null;
        }

        public override object VisitPropertyDeclaration (PropertyDeclaration node, object data)
        {
            var attr = GetPropertyAttribute (node, "NativeInline");
            
            if (attr != null) {
                Console.WriteLine ("NativeInline: " + attr.NamedArguments[0]);
            }
            
            var isStatic = TypeInfo.StaticProperties.ContainsKey (node.Name);
            
            if (node.HasGetRegion) {
                Write ("get");
                if (!isStatic) {
                    Write (" ", node.Name, "() ");
                } else {
                    Write (": function() ");
                }
                node.GetRegion.AcceptVisitor (this, null);
                if (node.HasSetRegion) {
                    Write (",");
                    NewLine ();
                }
            }
            if (node.HasSetRegion) {
                Write ("set");
                if (!isStatic) {
                    Write (" ", node.Name, "(value) ");
                } else {
                    Write (": function(value) ");
                }
                // TODO: Set stack
                node.SetRegion.AcceptVisitor (this, null);
            }
            
            return null;
        }

        public override object VisitPropertyGetRegion (PropertyGetRegion node, object data)
        {
            if (!node.Block.IsNull && !node.IsNull) {
                node.Block.AcceptVisitor (this, null);
            } else {
                Write ("{ ");
                var name = ((PropertyDeclaration)node.Parent).Name;
                if (((PropertyDeclaration)node.Parent).TypeReference.Type == "System.Int32") {
                    Write ("if (typeof(this.$", name, ") == 'undefined') { return 0; } ");
                }
                Write (" return this.$" + name + "; }");
            }
            return null;
        }

        public override object VisitPropertySetRegion (PropertySetRegion node, object data)
        {
            if (!node.Block.IsNull && !node.IsNull) {
                node.Block.AcceptVisitor (this, null);
            } else {
                var name = ((PropertyDeclaration)node.Parent).Name;
                Write ("{ this.$", name, " = value; }");
            }
            return null;
        }


        public override object VisitQueryExpression (QueryExpression queryExpression, object data)
        {
            
            //Write ("E.From(");
            //Write (queryExpression.FromClause.Identifier);
            //Write (" in ");
            var t = (Mono.Cecil.GenericInstanceType)queryExpression.FromClause.InExpression.AcceptVisitor (this, null);
            Locals.Add (queryExpression.FromClause.Identifier, t.GenericArguments[0]);
            
            //Write (")");
            Indent ();
            foreach (var clause in queryExpression.MiddleClauses) {
                NewLine ();
                Write (".CreateQuery(");
                clause.AcceptVisitor (LinqExpressionEmitter, null);
                Write (")");
            }
            NewLine ();
            Write (".CreateQuery(");
            
            var type = queryExpression.SelectOrGroupClause.AcceptVisitor (LinqExpressionEmitter, null);
            Write (")");
            Outdent ();
            var ien = KnownTypes["System.Collections.Generic.IEnumerable"];
            
            //Console.WriteLine("IEN: {0}", ien.GenericParameters[0]);
            //Console.WriteLine ("R: {0}", type);
            return ien;
        }

        /*
        public override object VisitQueryExpressionWhereClause (QueryExpressionWhereClause node, object data)
        {
            var parent = (QueryExpression)node.Parent;
            Write (".Where(function("+parent.FromClause.Identifier+") { return ");
            node.Condition.AcceptVisitor (this, null);
            Write (";})");
            return null;
        }
        
        public override object VisitQueryExpressionSelectClause (QueryExpressionSelectClause node, object data)
        {
            var parent = (QueryExpression)node.Parent;
            Write (".Select(function(" + parent.FromClause.Identifier + ") { return ");
            var type = node.Projection.AcceptVisitor (this, null);
            Write (";})");
            Console.WriteLine ("TYPE: {0}", type);
            return type;
        }*/



        public override object VisitExpressionStatement (ExpressionStatement node, object data)
        {
            if (node.IsNull)
                return null;
            
            node.Expression.AcceptVisitor (this, null);
            
            if (EnableSemicolon) {
                Write (";");
                NewLine ();
            }
            return null;
        }

        public override object VisitEmptyStatement (EmptyStatement node, object data)
        {
            Write (";");
            NewLine ();
            return null;
        }

        public override object VisitUnaryOperatorExpression (UnaryOperatorExpression node, object data)
        {
            switch (node.Op) {
            case UnaryOperatorType.BitNot:
                Write ("~");
                node.Expression.AcceptVisitor (this, null);
                break;
            case UnaryOperatorType.Decrement:
                Write ("--");
                node.Expression.AcceptVisitor (this, null);
                break;
            case UnaryOperatorType.Increment:
                Write ("++");
                node.Expression.AcceptVisitor (this, null);
                Write ("");
                break;
            case UnaryOperatorType.Minus:
                Write ("-");
                node.Expression.AcceptVisitor (this, null);
                break;
            case UnaryOperatorType.Not:
                Write ("!");
                node.Expression.AcceptVisitor (this, null);
                break;
            case UnaryOperatorType.Plus:
                node.Expression.AcceptVisitor (this, null);
                break;
            case UnaryOperatorType.PostDecrement:
                node.Expression.AcceptVisitor (this, null);
                Write ("--");
                break;
            case UnaryOperatorType.PostIncrement:
                node.Expression.AcceptVisitor (this, null);
                Write ("++");
                break;
            default:
                throw CreateException (node, "Unsupported unary operator: " + node.Op.ToString ());
            }
            return null;
        }

        public override object VisitUncheckedExpression (UncheckedExpression uncheckedExpression, object data)
        {
            uncheckedExpression.Expression.AcceptChildren (this, null);
            Console.WriteLine ("UncheckedExpression, handle this...");
            
            return null;
        }

        public override object VisitBinaryOperatorExpression (BinaryOperatorExpression node, object data)
        {
            node.Left.AcceptVisitor (this, null);
            Write (" ");
            switch (node.Op) {
            case BinaryOperatorType.Add:
                Write ("+");
                break;
            case BinaryOperatorType.BitwiseAnd:
                Write ("&");
                break;
            case BinaryOperatorType.BitwiseOr:
                Write ("|");
                break;
            case BinaryOperatorType.Divide:
                Write ("/");
                break;
            case BinaryOperatorType.Equality:
                Write ("==");
                break;
            case BinaryOperatorType.ExclusiveOr:
                Write ("^");
                break;
            case BinaryOperatorType.GreaterThan:
                Write (">");
                break;
            case BinaryOperatorType.GreaterThanOrEqual:
                Write (">=");
                break;
            case BinaryOperatorType.InEquality:
                Write ("!=");
                break;
            case BinaryOperatorType.LessThan:
                Write ("<");
                break;
            case BinaryOperatorType.LessThanOrEqual:
                Write ("<=");
                break;
            case BinaryOperatorType.LogicalAnd:
                Write ("&&");
                break;
            case BinaryOperatorType.LogicalOr:
                Write ("||");
                break;
            case BinaryOperatorType.Modulus:
                Write ("%");
                break;
            case BinaryOperatorType.Multiply:
                Write ("*");
                break;
            case BinaryOperatorType.ShiftLeft:
                Write ("<<");
                break;
            case BinaryOperatorType.ShiftRight:
                Write (">>");
                break;
            case BinaryOperatorType.Subtract:
                Write ("-");
                break;
            case BinaryOperatorType.NullCoalescing:
                Write ("??");
                break;
            default:
                throw CreateException (node, "Unsupported binary operator: " + node.Op.ToString ());
            }
            Write (" ");
            node.Right.AcceptVisitor (this, null);
            return null;
        }


        public override object VisitIdentifierExpression (IdentifierExpression node, object data)
        {
            var id = node.Identifier;
            CheckIdentifier (id, node);
            
            if (Locals.ContainsKey (id)) {
                Write (id);
                return Locals[id];
            }
            
            string typeName = ResolveType (id);
            
            if (!string.IsNullOrEmpty (typeName)) {
                var type = KnownTypes[typeName];
                if (type.Name == id) {
                    
                    var attr = GetAttribute ("NativeInline", type.CustomAttributes);
                    if (attr != null) {
                        var inline = attr.ConstructorArguments[0].Value;
                        Console.WriteLine ("Inline: " + inline);
                    } else {
                        attr = GetAttribute ("Native", type.CustomAttributes);
                        if (attr != null) {
                            //Console.WriteLine ("Native: {0}", (attr.ConstructorArguments[0].Value));
                            Write (((Mono.Cecil.CustomAttributeArgument[])attr.ConstructorArguments[0].Value)[0].Value);
                            Console.WriteLine ("DONe {0}", type);
                            return type;
                        }
                    }
                    //Console.WriteLine ("Identifier type:" + type);
                }
            }
            
                        /* Instance method */
var isMethod = (from m in TypeInfo.InstanceMethods
                where m.Key == id
                select m).Count () > 0;
            if (isMethod) {
                WriteThis ();
                Write (".", id, ".bind(this)");
                return null;
            }
            
                        /* Instance field */
if (TypeInfo.InstanceFields.ContainsKey (id)) {
                Write ("this.", id);
                return TypeInfo.InstanceFields[id].Value.TypeReference;
            }
            
                        /* Instance property */
if (TypeInfo.InstanceProperties.ContainsKey (id)) {
                Write ("this.", id);
                return TypeInfo.InstanceProperties[id].TypeReference;
            }
            
                        /* Static field */
var isStaticField = (from f in TypeInfo.StaticFields
                where f.Key == id
                select f).Count () == 1;
            
            if (isStaticField) {
                Write (TypeInfo.FullName + "." + id);
                return null;
            }
            
            
            
                        /* Static method */
var isStaticMethod = (from m in TypeInfo.StaticMethods
                where m.Key == id
                select m).Count () > 0;
            if (isStaticMethod) {
                Write (TypeInfo.FullName, ".", id);
                return null;
            }
            
            foreach (var u in TypeInfo.Usings) {
                if (KnownTypes.ContainsKey (u + "." + id)) {
                    Write (u + "." + id);
                    return null;
                }
            }
            
            Write (id);
            Console.WriteLine ("Cannot resolve identifier: " + id);
            //throw CreateException(node, "Cannot resolve identifier: " + id);
            return null;
        }

        public CustomAttribute GetAttribute (string attributeTypeName, Mono.Collections.Generic.Collection<CustomAttribute> attributes)
        {
            if (attributes == null) {
                return null;
            }
            var name = attributeTypeName + "Attribute";
            foreach (CustomAttribute attr in attributes) {
                //Console.WriteLine ("Attr: " + attr.Constructor.DeclaringType.Name);
                if (attr.Constructor.DeclaringType.Name == name) {
                    return attr;
                }
            }
            return null;
        }

        public override object VisitMemberReferenceExpression (MemberReferenceExpression node, object data)
        {
            Mono.Cecil.TypeReference returnType = null;
            
            
            
            var type = ResolveOwnerType (node);
            //if (node.MemberName == "Length") {
            //Console.WriteLine ("MemberReferenceNode: {0} (type: {1})", node, type);
            //}
            
            if (type != null) {
                
                //if ((node.TargetObject is IdentifierExpression && Locals.ContainsKey (((IdentifierExpression)node.TargetObject).Identifier))
                //|| !(node.TargetObject is IdentifierExpression) || !IsHidden (type)) {
                
                //Console.WriteLine("visit TargetObject {0}", node.TargetObject);
                node.TargetObject.AcceptVisitor (this, null);
                
                Write (".");
                //}
                var tr = type as TypeDefinition;
                
                var trr = type as ICSharpCode.NRefactory.Ast.TypeReference;
                if (trr != null) {
                    if (trr.IsArrayType) {
                        tr = KnownTypes["System.Array"];
                    }
                }
                
                
                if (IsEvent (node)) {
                    if (tr != null) {
                        var evt = GetEvent (node.MemberName, tr);
                        
                        return evt.EventType;
                    }
                }
                
                //Console.WriteLine ("tr: {0} {1}", tr, node);
                
                Mono.Collections.Generic.Collection<CustomAttribute> customAttributes = null;
                if (tr != null) {
                    var property = GetProperty (node.MemberName, tr);
                    
                    if (property != null) {
                        customAttributes = property.CustomAttributes;
                        returnType = property.PropertyType;
                    } else {
                        var method = GetMethod (node.MemberName, tr);
                        
                        //Console.WriteLine ("GetMethod: {0}, {1}", node.MemberName, method);
                        if (method != null) {
                            customAttributes = method.CustomAttributes;
                            returnType = method.ReturnType;
                        } else {
                            var field = GetField (node.MemberName, tr);
                            
                            if (field != null) {
                                customAttributes = field.CustomAttributes;
                                returnType = field.FieldType;
                            }
                        }
                    }
                }
                
                var attr = GetAttribute ("NativeInline", customAttributes);
                if (attr != null) {
                    Write (attr.ConstructorArguments[0].Value);
                } else {
                    attr = GetAttribute ("Native", customAttributes);
                    if (attr != null) {
                        var lines = (Mono.Cecil.CustomAttributeArgument[])attr.ConstructorArguments[0].Value;
                        foreach (var line in lines) {
                            Write (line.Value);
                        }
                        
                        
                        //Write(String.Join ("\n", lines));
                    }
                }
                
                if (attr == null) {
                    Write (GenericsHelper.GetScriptName (node));
                }
            } else {
                var ns = GetFullName (node);
                if (ns != null && KnownTypes.ContainsKey (ns)) {
                    returnType = KnownTypes[ns];
                    var kt = (Mono.Cecil.TypeDefinition)returnType;
                    var attr = GetAttribute ("Native", kt.CustomAttributes);
                    if (attr != null) {
                        var lines = (Mono.Cecil.CustomAttributeArgument[])attr.ConstructorArguments[0].Value;
                        foreach (var line in lines) {
                            Write (line.Value);
                        }
                    } else {
                        Write (ns);
                    }
                } else {
                    Console.WriteLine ("Couldn't determine type of member reference: {0}", node);
                    node.TargetObject.AcceptVisitor (this, null);
                    
                    Write (".");
                    Write (node.MemberName);
                }
            }
            
            if (returnType == null) {
                var ns = GetFullName (node);
                if (ns != null) {
                    //Console.WriteLine ("NNNN: " + ns);
                    Write (ns);
                }
            }
            
            //  Write (".");
            
            //Write (GenericsHelper.GetScriptName (node));
            return returnType;
        }

        public bool IsHidden (TypeDefinition type)
        {
            var attr = GetAttribute ("Hidden", type.CustomAttributes);
            return attr != null;
        }

        public object ResolveOwnerType (MemberReferenceExpression node)
        {
            var identifier = node.TargetObject as IdentifierExpression;
            if (identifier != null) {
                //if (string.IsNullOrEmpty (typeName)) {
                CheckIdentifier (identifier.Identifier, identifier);
                if (Locals.ContainsKey (identifier.Identifier)) {
                    var type = Locals[identifier.Identifier];
                    
                    //Console.WriteLine ("TT: {0}", type);
                    
                    var astType = type as ICSharpCode.NRefactory.Ast.TypeReference;
                    var cecilType = type as Mono.Cecil.TypeReference;
                    
                    string resolvedName = null;
                    if (astType != null) {
                        if (astType.IsArrayType) {
                            return KnownTypes["System.Array"];
                        }
                        resolvedName = ResolveType (astType.Type);
                    }
                    if (cecilType != null) {
                        if (cecilType.Name.IndexOf ("[]") > 0) {
                            return KnownTypes["System.Array"];
                        }
                        if (KnownTypes.ContainsKey (GenericsHelper.GetScriptFullName (cecilType.FullName))) {
                            return KnownTypes[GenericsHelper.GetScriptFullName (cecilType.FullName)];
                        }
                        resolvedName = ResolveType (cecilType.FullName);
                        
                    }
                    
                    if (!string.IsNullOrEmpty (resolvedName) && KnownTypes.ContainsKey (resolvedName)) {
                        return KnownTypes[resolvedName];
                    }
                }
                //}
                
                var typeName = ResolveType (identifier.Identifier);
                if (!string.IsNullOrEmpty (typeName) && KnownTypes.ContainsKey (typeName)) {
                    var type = KnownTypes[typeName];
                    if (type != null) {
                        return type;
                    }
                    
                    
                }
                
                //typeName =
                
                if (TypeInfo.StaticFields.ContainsKey (identifier.Identifier)) {
                    var field = (FieldDeclaration)TypeInfo.StaticFields[identifier.Identifier].Value;
                    var t = ResolveType (field.TypeReference.Type);
                    if (KnownTypes.ContainsKey (t)) {
                        return KnownTypes[t];
                    } else {
                        Console.WriteLine ("Type: {0} is not in KnownTypes.", t);
                    }
                }
                
                var ns = ResolveNamespaceOrType (identifier.Identifier, true);
                if (!string.IsNullOrEmpty (ns)) {
                    Console.WriteLine ("NS: " + ns);
                    //throw new Exception ();
                    return null;
                }
                
                Console.WriteLine ("Identifier not resolved: " + identifier.Identifier);
            }
            
            var member = node.TargetObject as MemberReferenceExpression;
            if (member != null) {
                var ns = GetFullName (member);
                if (ns != null) {
                    //Console.WriteLine ("NAMESPACE: " + ns);
                    if (KnownTypes.ContainsKey (ns)) {
                        return KnownTypes[ns];
                    } else if (KnownNamespaces.Contains (ns)) {
                        //Console.WriteLine ("{0} is a namespace, not a type. Member: {1}", ns, member);
                        return null;
                    } else {
                        Console.WriteLine ("{0} not found in KnownTypes. Member: {1}", ns, member);
                        throw new Exception ();
                    }
                    //throw new Exception ();
                } else {
                    var owner = ResolveOwnerType (member);
                    var tr = owner as TypeDefinition;
                    var trr = owner as ICSharpCode.NRefactory.Ast.TypeReference;
                    
                    if (tr != null) {
                        var property = GetProperty (member.MemberName, tr);
                        if (property != null) {
                            if (KnownTypes.ContainsKey (property.PropertyType.FullName)) {
                                return KnownTypes[property.PropertyType.FullName];
                            } else {
                                Console.WriteLine ("PropertyType not in KnownTypes");
                            }
                        } else if (member.TargetObject is ThisReferenceExpression && TypeInfo.InstanceProperties.ContainsKey (member.MemberName)) {
                            return tr;
                        } else if (member.TargetObject is ThisReferenceExpression && TypeInfo.InstanceFields.ContainsKey (member.MemberName)) {
                            return tr;
                        } else {
                            Console.WriteLine ("Property not found: {0} on Type: {1}", member.MemberName, tr);
                        }
                    }
                    
                    if (trr != null) {
                        if (member.TargetObject is ThisReferenceExpression && TypeInfo.InstanceFields.ContainsKey (member.MemberName)) {
                            return trr;
                        }
                    }
                    
                    Console.WriteLine ("OwnerType not found for: {0} ({1})", member.MemberName, owner.GetType ());
                }
            }
            
            var te = node.TargetObject as ThisReferenceExpression;
            if (te != null) {
                //Console.WriteLine ("***This");
                ICSharpCode.NRefactory.Ast.TypeReference tr = null;
                
                if (TypeInfo.InstanceMethods.ContainsKey (node.MemberName)) {
                    tr = TypeInfo.InstanceMethods[node.MemberName].TypeReference;
                } else if (TypeInfo.InstanceProperties.ContainsKey (node.MemberName)) {
                    var t = ResolveType (TypeInfo.InstanceProperties[node.MemberName].TypeReference.Type);
                    if (t != null && KnownTypes.ContainsKey (t)) {
                        return KnownTypes[t];
                    } else {
                        Console.WriteLine ("Property has unkown type: {0}", node.MemberName);
                    }
                } else if (TypeInfo.InstanceFields.ContainsKey (node.MemberName)) {
                    return TypeInfo.InstanceFields[node.MemberName].Value.TypeReference;
                    
                    var t = ResolveType (TypeInfo.InstanceFields[node.MemberName].Value.TypeReference.Type);
                    
                    if (t != null && KnownTypes.ContainsKey (t)) {
                        //Console.WriteLine ("T: {0}", t);
                        return KnownTypes[t];
                    } else {
                        Console.WriteLine ("Field has unkown type: {0}", node.MemberName);
                    }
                }
                
                if (tr != null) {
                    var r = GenericsHelper.GetScriptFullName (tr.Type);
                    
                    if (KnownTypes.ContainsKey (r)) {
                        var t = KnownTypes[r];
                        Console.WriteLine ("Instancemethod: {0}", t);
                        return t;
                    } else if (node.TypeArguments.Count () == 1) {
                        // TODO: Sjekk at retur typen er samme som TypeArguments[0]
                        r = GenericsHelper.GetScriptFullName (ResolveType (node.TypeArguments[0].Type));
                        if (KnownTypes.ContainsKey (r)) {
                            Console.WriteLine ("Return: {0}", KnownTypes[r]);
                            return KnownTypes[r];
                        } else {
                            Console.WriteLine ("Type {0} is not in known types.", node.MemberName);
                        }
                    } else {
                        Console.WriteLine ("Not found: {0}", r);
                    }
                } else {
                    Console.WriteLine ("Method or Property on ThisReference not found, name: {0}", node.MemberName);
                }
            }
            
            var invocation = node.TargetObject as InvocationExpression;
            if (invocation != null) {
                Console.WriteLine ("Not implemented: ResolveOwnerType with invocation");
            }
            
            if (node is MemberReferenceExpression) {
                var ns = GetFullName (node);
                if (ns != null && KnownTypes.ContainsKey (ns)) {
                    return null;
                }
            }
            
            Console.WriteLine ("Not found ownerType: " + node);
            
            return null;
        }

        public string GetFullName (MemberReferenceExpression member)
        {
            if (member.TargetObject is MemberReferenceExpression) {
                var ns = GetFullName ((MemberReferenceExpression)member.TargetObject);
                if (ns != null) {
                    return ns + "." + member.MemberName;
                }
            } else if (member.TargetObject is IdentifierExpression) {
                
                var name = ((IdentifierExpression)member.TargetObject).Identifier;
                if (KnownNamespaces.Contains (name)) {
                    return name + "." + member.MemberName;
                }
            }
            return null;
        }

        public PropertyDefinition GetProperty (string propertyName, TypeDefinition type)
        {
            foreach (PropertyDefinition property in type.Properties) {
                if (property.Name == propertyName) {
                    return property;
                }
            }
            
            if (type.BaseType != null) {
                if (KnownTypes.ContainsKey (type.BaseType.FullName)) {
                    var t = KnownTypes[type.BaseType.FullName];
                    return GetProperty (propertyName, t);
                }
            }
            
            return null;
        }

        public MethodDefinition GetMethod (string methodName, TypeDefinition type)
        {
            foreach (MethodDefinition method in type.Methods) {
                if (method.Name == methodName) {
                    return method;
                }
            }
            
            if (type.BaseType != null) {
                if (KnownTypes.ContainsKey (type.BaseType.FullName)) {
                    var t = KnownTypes[type.BaseType.FullName];
                    return GetMethod (methodName, t);
                }
            }
            
            return null;
        }

        public EventDefinition GetEvent (string eventName, TypeDefinition type)
        {
            foreach (EventDefinition e in type.Events) {
                if (e.Name == eventName) {
                    return e;
                }
            }
            
            if (type.BaseType != null) {
                if (KnownTypes.ContainsKey (type.BaseType.FullName)) {
                    var t = KnownTypes[type.BaseType.FullName];
                    return GetEvent (eventName, t);
                }
            }
            
            return null;
        }

        public FieldDefinition GetField (string fieldName, TypeDefinition type)
        {
            foreach (FieldDefinition f in type.Fields) {
                if (f.Name == fieldName) {
                    return f;
                }
            }
            
            if (type.BaseType != null) {
                if (KnownTypes.ContainsKey (type.BaseType.FullName)) {
                    var t = KnownTypes[type.BaseType.FullName];
                    return GetField (fieldName, t);
                }
            }
            
            return null;
        }

        public override object VisitThisReferenceExpression (ThisReferenceExpression node, object data)
        {
            WriteThis ();
            return null;
        }

        public override object VisitBaseReferenceExpression (BaseReferenceExpression node, object data)
        {
            throw CreateException (node, "No need to use base qualifier here");
        }

        public override object VisitInvocationExpression (InvocationExpression node, object data)
        {
            //Console.WriteLine ("**** Invocation: {0}", node);
            string nativeFormat = GetNativeInlineImpl (node);
            if (!string.IsNullOrEmpty (nativeFormat)) {
                //Console.WriteLine ("NATIVEINLINE");
                //node.TargetObject.AcceptVisitor (this, null);
                //Console.WriteLine ("**** TargetObject: {0}", node.TargetObject);
                Write ("");
                StringBuilder savedBuilder = Output;
                var args = new List<string> (node.Arguments.Count);
                foreach (var arg in node.Arguments) {
                    Output = new StringBuilder ();
                    arg.AcceptVisitor (this, null);
                    args.Add (Output.ToString ());
                }
                Output = savedBuilder;
                Output.Append (String.Format (nativeFormat, args.ToArray ()));
                return null;
            }
            
            MemberReferenceExpression targetMember = node.TargetObject as MemberReferenceExpression;
            if (targetMember != null && targetMember.TargetObject is BaseReferenceExpression) {
                var baseType = GetBaseMethodOwnerTypeDefinition (targetMember.MemberName, targetMember.TypeArguments.Count);
                if (JsMetadataChecker.HasHiddenAttribute (baseType))
                    throw CreateException (targetMember.TargetObject, "Cannot call base method, because parent class is hidden");
                Write (GenericsHelper.GetScriptFullName (baseType), ".", GenericsHelper.GetScriptName (targetMember));
                Write (".call(");
                WriteThis ();
                foreach (var arg in node.Arguments) {
                    WriteComma ();
                    arg.AcceptVisitor (this, null);
                }
                Write (")");
            } else {
                node.TargetObject.AcceptVisitor (this, null);
                Write ("(");
                EmitExpressionList (node.Arguments);
                Write (")");
            }
            
            //Console.WriteLine ("TargetMemeber: {0}", targetMember);
            
            if (targetMember != null) {
                var type = ResolveOwnerType (targetMember);
                if (type != null) {
                    //return type;
                    var tt = type as Mono.Cecil.TypeDefinition;
                    var tr = type as ICSharpCode.NRefactory.Ast.TypeReference;
                    if (tt != null) {
                        var method = (from m in tt.Methods
                            where m.Name == targetMember.MemberName
                            select m).ToArray ();
                        if (method != null && method.Length > 0) {
                            return method[0].ReturnType;
                            
                            
                        } else {
                            Console.WriteLine ("Method not found on type: {0}", targetMember);
                            return type;
                        }
                    } else if (tr != null) {
                        var typeName = ResolveType (tr.Type);
                        if (KnownTypes.ContainsKey (typeName)) {
                            var t = KnownTypes[typeName];
                            var method = (from m in t.Methods
                                where m.Name == targetMember.MemberName
                                select m).ToArray ();
                            
                            if (method != null && method.Length > 0) {
                                return method[0].ReturnType;
                            }
                        }
                        //Console.WriteLine ("TODO: TypeDecleration {0}", type.GetType());
                    }
                } else {
                    return type;
                }
            }
            Console.WriteLine ("Unkown return type: {0}", node);
            return null;
        }

        public override object VisitAssignmentExpression (AssignmentExpression node, object data)
        {
            
            if ((node.Op == AssignmentOperatorType.Add || node.Op == AssignmentOperatorType.Subtract) && IsEvent (node.Left)) {
                CurrentEventType = (Mono.Cecil.TypeReference)node.Left.AcceptVisitor (this, null);
                
                Write ("addEventListener('" + GetEventName (node.Left) + "', ");
                node.Right.AcceptVisitor (this, null);
                CurrentEventType = null;
                Write (")");
            } else {
                node.Left.AcceptVisitor (this, null);
                Write (" ");
                switch (node.Op) {
                case AssignmentOperatorType.Assign:
                    break;
                case AssignmentOperatorType.Add:
                    Write ("+");
                    break;
                case AssignmentOperatorType.BitwiseAnd:
                    Write ("&");
                    break;
                case AssignmentOperatorType.BitwiseOr:
                    Write ("|");
                    break;
                case AssignmentOperatorType.Divide:
                    Write ("/");
                    break;
                case AssignmentOperatorType.ExclusiveOr:
                    Write ("^");
                    break;
                case AssignmentOperatorType.Modulus:
                    Write ("%");
                    break;
                case AssignmentOperatorType.Multiply:
                    Write ("*");
                    break;
                case AssignmentOperatorType.ShiftLeft:
                    Write ("<<");
                    break;
                case AssignmentOperatorType.ShiftRight:
                    Write (">>");
                    break;
                case AssignmentOperatorType.Subtract:
                    Write ("-");
                    break;
                default:
                    throw CreateException (node, "Unsupported assignment operator: " + node.Op.ToString ());
                }
                Write ("= ");
                var returnType = node.Right.AcceptVisitor (this, null);
            }
            return null;
        }

        public override object VisitTypeReferenceExpression (TypeReferenceExpression node, object data)
        {
            node.TypeReference.AcceptVisitor (this, null);
            return null;
        }

        public override object VisitTypeReference (AstTypeReference node, object data)
        {
            if (node.IsArrayType) {
                Write ("Array");
            } else {
                string type = ResolveType (GenericsHelper.GetScriptName (node));
                if (String.IsNullOrEmpty (type)) {
                    Console.WriteLine ("Cannot resolve type " + node.Type);
                    //throw CreateException (node, "Cannot resolve type " + node.Type);
                }
                if (type != null) {
                    var name = GetNativeName (KnownTypes[type].CustomAttributes);
                    //Console.WriteLine ("*** TYPE: {0} NAME: {1}", type, name);
                    if (string.IsNullOrEmpty (name)) {
                        name = ShortenTypeName (type);
                    }
                    Write (name);
                } else {
                    Write (node.Type);
                }
            }
            return node;
        }

        public override object VisitInnerClassTypeReference (InnerClassTypeReference node, object data)
        {
            VisitTypeReference (node, data);
            return null;
        }

        public override object VisitArrayCreateExpression (ArrayCreateExpression node, object data)
        {
            if (node.Arguments.Count > 1)
                throw CreateException (node, "Multi-dimensional arrays are not supported");
            Write ("[ ");
            var list = node.ArrayInitializer.CreateExpressions;
            EmitExpressionList (list);
            if (list.Count > 0)
                Write (" ");
            Write ("]");
            return null;
        }

        public override object VisitLambdaExpression (LambdaExpression node, object data)
        {
            Mono.Collections.Generic.Collection<ParameterDefinition> paramdefinitions = null;
            INode body;
            if (node.ExpressionBody.IsNull)
                body = node.StatementBody;
            else
                body = node.ExpressionBody;
            
            if (CurrentEventType != null) {
                //Console.WriteLine ("{0} {1}", KnownTypeInfos.Count, GenericsHelper.GetScriptFullName(CurrentEventType.DeclaringType));
                var type = KnownTypes[GenericsHelper.GetScriptFullName (CurrentEventType.DeclaringType)];
                TypeDefinition delg = null;
                foreach (TypeDefinition m in type.NestedTypes) {
                    if (m.FullName == CurrentEventType.FullName) {
                        delg = m;
                        break;
                    }
                    //Console.WriteLine ("Nested Type: {0}", m.Name);
                }
                //var known = type.InstanceDelegates.ContainsKey (CurrentEventType.Name);
                paramdefinitions = delg.Methods[0].Parameters;
            }
            //node.Parameters[0].TypeReference = null;
            
            // set CurrentEventType to null since we only need it to get the parameter types,
            // and so that we can support nested lambda expressions
            CurrentEventType = null;
            
            //Console.WriteLine("Lambda: {0}", node.Parameters);
            
            EmitLambda (node.Parameters, body, node, paramdefinitions);
            
            return null;
        }

        public override object VisitAnonymousMethodExpression (AnonymousMethodExpression node, object data)
        {
            EmitLambda (node.Parameters, node.Body, node, null);
            return null;
        }

        public override object VisitObjectCreateExpression (ObjectCreateExpression node, object data)
        {
            //if(node.IsAnonymousType)
            //    throw CreateException(node, "Anonymous types are not supported");
            
            
            Mono.Cecil.TypeDefinition type = null;
            if (node.CreateType != null) {
                type = GetTypeDefinition (node.CreateType);
            }
            if (type != null && type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate") {
                node.Parameters[0].AcceptVisitor (this, null);
                return node.CreateType;
            }
            
            var customCtor = (type != null && JsMetadataChecker.GetCustomConstructor (type) != null) ? JsMetadataChecker.GetCustomConstructor (type) : "";
            var hasInitializer = !node.ObjectInitializer.IsNull && node.ObjectInitializer.CreateExpressions.Count > 0;
            
            if (Regex.Match (customCtor, @"\s*\{\s*\}\s*").Success) {
                Write ("{ ");
                if (hasInitializer) {
                    WriteObjInitializer (node.ObjectInitializer.CreateExpressions);
                    Write (" }");
                } else {
                    Write ("}");
                }
            } else {
                if (hasInitializer)
                    Write ("Object.create(");
                if (String.IsNullOrEmpty (customCtor)) {
                    Write ("new ");
                    if (type == null) {
                        Write ("Object");
                    }
                    node.CreateType.AcceptVisitor (this, null);
                } else {
                    if (type != null && !JsMetadataChecker.HasHiddenAttribute (type))
                        throw new ApplicationException (string.Format ("Custom constructors are only allowed for Hidden types: {0}", type));
                    Write (customCtor);
                }
                Write ("(");
                EmitExpressionList (node.Parameters);
                Write (")");
                if (hasInitializer) {
                    WriteComma ();
                    Write ("{ ");
                    var list = node.ObjectInitializer.CreateExpressions;
                    foreach (NamedArgumentExpression item in list) {
                        if (item != list[0])
                            WriteComma ();
                        Write (item.Name, ": { value: ");
                        item.Expression.AcceptVisitor (this, null);
                        Write (" }");
                    }
                    Write (" }");
                    Write (")");
                }
            }
            
            return node.CreateType;
        }


        public override object VisitIfElseStatement (IfElseStatement node, object data)
        {
            if (node.TrueStatement.Count > 1 || node.FalseStatement.Count > 1)
                throw CreateException (node, "Too many statements");
            
            Write ("if(");
            node.Condition.AcceptVisitor (this, null);
            Write (")");
            EmitBlockOrIndentedLine (node.TrueStatement[0]);
            node.ElseIfSections.ForEach (s => s.AcceptVisitor (this, null));
            if (node.FalseStatement.Count > 0) {
                Write ("else");
                EmitBlockOrIndentedLine (node.FalseStatement[0]);
            }
            return null;
        }

        public override object VisitElseIfSection (ElseIfSection node, object data)
        {
            Write ("else if(");
            node.Condition.AcceptVisitor (this, null);
            Write (")");
            EmitBlockOrIndentedLine (node.EmbeddedStatement);
            return null;
        }

        public override object VisitForStatement (ForStatement node, object data)
        {
            if (node.Initializers.Count > 1)
                throw CreateException (node, "Too many initializers");
            
            PushLocals ();
            
            EnableSemicolon = false;
            Write ("for(");
            
            foreach (var item in node.Initializers) {
                if (item != node.Initializers[0])
                    WriteComma ();
                item.AcceptVisitor (this, null);
            }
            Write ("; ");
            
            node.Condition.AcceptVisitor (this, null);
            Write ("; ");
            
            foreach (var item in node.Iterator) {
                if (item != node.Iterator[0])
                    WriteComma ();
                item.AcceptVisitor (this, null);
            }
            Write (")");
            EnableSemicolon = true;
            
            EmitBlockOrIndentedLine (node.EmbeddedStatement);
            
            PopLocals ();
            return null;
        }

        public override object VisitIndexerDeclaration (IndexerDeclaration node, object data)
        {
            if (node.HasGetRegion) {
                Write ("get: function(");
                Write (node.Parameters[0].ParameterName);
                Write (")");
                PushLocals ();
                Locals.Add (node.Parameters[0].ParameterName, node.Parameters[0].TypeReference);
                node.GetRegion.Block.AcceptVisitor (this, null);
                PopLocals ();
                
                if (node.HasSetRegion) {
                    WriteComma ();
                    NewLine ();
                    NewLine ();
                }
            }
            
            if (node.HasSetRegion) {
                Write ("set: function(");
                Write (node.Parameters[0].ParameterName);
                Write (", value)");
                PushLocals ();
                Locals.Add (node.Parameters[0].ParameterName, node.Parameters[0].TypeReference);
                Locals.Add ("value", node.TypeReference);
                node.SetRegion.Block.AcceptVisitor (this, null);
                PopLocals ();
            }
            
            return node.TypeReference;
        }

        public override object VisitIndexerExpression (IndexerExpression node, object data)
        {
            if (node.Indexes.Count != 1)
                throw CreateException (node, "Only one index is supported");
            
            var type = node.TargetObject.AcceptVisitor (this, null);
            
            var index = node.Indexes[0];
            
            var primitive = index as PrimitiveExpression;
            if (primitive != null && primitive.Value != null && Regex.Match (primitive.Value.ToString (), "^[_$a-z][_$a-z0-9]*$", RegexOptions.IgnoreCase).Success) {
                Write (".", primitive.Value);
            } else {
                Write ("[");
                index.AcceptVisitor (this, null);
                Write ("]");
            }
            
            return type;
        }

        public override object VisitCastExpression (CastExpression node, object data)
        {
            var castToTypeIsHidden = false;
            
            var type = ResolveType (node.CastTo.Type);
            if (type != null) {
                castToTypeIsHidden = IsHidden (KnownTypes[type]);
            }
            if (!castToTypeIsHidden) {
                //Write (RUNTIME_HELPER_NAME);
                //Write (".");
                switch (node.CastType) {
                case CastType.Cast:
                    //Write (CAST_NAME);
                    break;
                case CastType.TryCast:
                    //Write (TRYCAST_NAME);
                    break;
                default:
                    throw CreateException (node, "Unsupported cast type");
                }
                //Write ("(");
            }
            node.Expression.AcceptVisitor (this, null);
            object returnType = node.CastTo;
            if (!castToTypeIsHidden) {
                //WriteComma ();
                
                //node.CastTo.AcceptVisitor (this, false);
                
                //Write (")");
            }
            
            return returnType;
        }

        public override object VisitTypeOfIsExpression (TypeOfIsExpression node, object data)
        {
            Write (RUNTIME_HELPER_NAME, ".", IS_NAME, "(");
            node.Expression.AcceptVisitor (this, null);
            WriteComma ();
            node.TypeReference.AcceptVisitor (this, null);
            Write (")");
            return null;
        }

        public override object VisitReturnStatement (ReturnStatement node, object data)
        {
            Write ("return");
            if (!node.Expression.IsNull) {
                Write (" ");
                node.Expression.AcceptVisitor (this, null);
            }
            Write (";");
            NewLine ();
            return null;
        }

        public override object VisitThrowStatement (ThrowStatement node, object data)
        {
            Write ("throw ");
            node.Expression.AcceptVisitor (this, null);
            Write (";");
            NewLine ();
            return null;
        }

        public override object VisitForeachStatement (ForeachStatement node, object data)
        {
            if (node.EmbeddedStatement is EmptyStatement)
                return null;
            
            var iteratorName = GetNextIteratorName ();
            
            Write ("var ", iteratorName, " = ");
            var extType = node.Expression.AcceptVisitor (this, null);
            
            var ctype = extType as Mono.Cecil.TypeReference;
            var itype = extType as ICSharpCode.NRefactory.Ast.TypeReference;
            
            var isArray = false;
            
            if (ctype != null) {
                isArray = ctype.FullName.IndexOf ("[]") > 0;
            } else if (itype != null) {
                isArray = itype.IsArrayType;
            }
            
            //Console.WriteLine ("Node: {0}", name);
            if (isArray) {
                Write (";");
                NewLine ();
                Write ("for(var $j=0;$j < ", iteratorName, ".length;$j++)");
                
                NewLine ();
                BeginBlock ();
                PushLocals ();
                Write ("var ", node.VariableName, "=", iteratorName, "[$j];");
                NewLine ();
                Locals.Add (node.VariableName, node.TypeReference);
                
                BlockStatement block = node.EmbeddedStatement as BlockStatement;
                if (block != null) {
                    block.AcceptChildren (this, null);
                } else {
                    node.EmbeddedStatement.AcceptVisitor (this, null);
                }
                PopLocals ();
                EndBlock ();
                NewLine ();
            } else {
                Write (".", CREATE_ITERATOR_NAME, "();");
                
                NewLine ();
                
                Write ("while(", iteratorName, ".", ITERATOR_HAS_NEXT_NAME, "()", ") ");
                BeginBlock ();
                
                Write ("var ", node.VariableName, " = ", iteratorName, ".", ITERATOR_NEXT_NAME, ";");
                NewLine ();
                
                PushLocals ();
                Locals.Add (node.VariableName, node.TypeReference);
                
                BlockStatement block = node.EmbeddedStatement as BlockStatement;
                if (block != null) {
                    block.AcceptChildren (this, null);
                } else {
                    node.EmbeddedStatement.AcceptVisitor (this, null);
                }
                
                PopLocals ();
                
                EndBlock ();
                NewLine ();
            }
            return null;
        }

        public override object VisitConditionalExpression (ConditionalExpression node, object data)
        {
            node.Condition.AcceptVisitor (this, null);
            Write (" ? ");
            node.TrueExpression.AcceptVisitor (this, null);
            Write (" : ");
            var type = node.FalseExpression.AcceptVisitor (this, null);
            
            //Console.WriteLine ("Condi Type: {0}", type);
            
            return type;
        }

        public override object VisitTryCatchStatement (TryCatchStatement node, object data)
        {
            if (node.CatchClauses.Count > 1)
                throw CreateException (node, "Multiple catch clauses are not supported");
            
            Write ("try ");
            node.StatementBlock.AcceptVisitor (this, null);
            foreach (var clause in node.CatchClauses) {
                PushLocals ();
                
                if (!clause.TypeReference.IsNull) {
                    if (ResolveType (clause.TypeReference.Type) != "System.Exception")
                        throw CreateException (node, "Only System.Exception type is allowed in catch clauses");
                }
                var varName = clause.VariableName;
                if (String.IsNullOrEmpty (varName)) {
                    varName = "$e";
                } else {
                    Locals.Add (varName, null);
                }
                Write ("catch(", varName, ") ");
                clause.StatementBlock.AcceptVisitor (this, null);
                
                PopLocals ();
            }
            if (!node.FinallyBlock.IsNull) {
                Write ("finally ");
                node.FinallyBlock.AcceptVisitor (this, null);
            }
            
            return null;
        }

        public override object VisitDirectionExpression (DirectionExpression directionExpression, object data)
        {
            directionExpression.Expression.AcceptChildren (this, null);
            Console.WriteLine ("Direction Expression, handle this...");
            return null;
        }

        public override object VisitDoLoopStatement (DoLoopStatement node, object data)
        {
            if (node.ConditionType != ConditionType.While)
                throw CreateException (node, "Unsupported condition type");
            
            switch (node.ConditionPosition) {
            case ConditionPosition.End:
                Write ("do");
                EmitBlockOrIndentedLine (node.EmbeddedStatement);
                if (node.EmbeddedStatement is BlockStatement)
                    Write (" ");
                Write ("while(");
                node.Condition.AcceptVisitor (this, null);
                Write (");");
                NewLine ();
                break;
            case ConditionPosition.Start:
                Write ("while(");
                node.Condition.AcceptVisitor (this, null);
                Write (")");
                EmitBlockOrIndentedLine (node.EmbeddedStatement);
                break;
            default:
                throw CreateException (node, "Unsupported condition position");
            }
            return null;
        }

        public override object VisitSwitchStatement (SwitchStatement node, object data)
        {
            Write ("switch(");
            node.SwitchExpression.AcceptVisitor (this, null);
            Write (") ");
            BeginBlock ();
            node.SwitchSections.ForEach (s => s.AcceptVisitor (this, null));
            EndBlock ();
            NewLine ();
            return null;
        }

        public override object VisitSwitchSection (SwitchSection node, object data)
        {
            node.SwitchLabels.ForEach (l => l.AcceptVisitor (this, null));
            Indent ();
            node.AcceptChildren (this, null);
            Outdent ();
            return null;
        }

        public override object VisitCaseLabel (CaseLabel node, object data)
        {
            if (node.IsDefault) {
                Write ("default");
            } else {
                Write ("case ");
                node.Label.AcceptVisitor (this, null);
            }
            Write (":");
            NewLine ();
            return null;
        }

        public override object VisitBreakStatement (BreakStatement node, object data)
        {
            Write ("break;");
            NewLine ();
            return null;
        }

        public override object VisitContinueStatement (ContinueStatement continueStatement, object data)
        {
            Write ("continue;");
            NewLine ();
            return null;
        }

        public override object VisitParenthesizedExpression (ParenthesizedExpression node, object data)
        {
            Write ("(");
            node.Expression.AcceptVisitor (this, null);
            Write (")");
            return null;
        }

        public override object VisitTypeOfExpression (TypeOfExpression node, object data)
        {
            Write ("new System.Type(");
            node.TypeReference.AcceptVisitor (this, null);
            Write (")");
            return KnownTypes["System.Type"];
        }

        public override object VisitUsingDeclaration (UsingDeclaration usingDeclaration, object data)
        {
            Write ("using(");
            foreach (var u in usingDeclaration.Usings) {
                u.AcceptChildren (this, null);
                if (usingDeclaration.Usings[usingDeclaration.Usings.Count - 1] != u) {
                    Write (", ");
                }
            }
            Write (");");
            
            return null;
        }

        public override object VisitUsing (Using @using, object data)
        {
            Write ("'" + @using.Name + '"');
            return null;
        }

        public override object VisitUsingStatement (UsingStatement usingStatement, object data)
        {
            usingStatement.EmbeddedStatement.AcceptChildren (this, null);
            usingStatement.ResourceAcquisition.AcceptChildren (this, null);
            
            return null;
        }
    }
}
