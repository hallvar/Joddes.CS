using System;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using Mono.Cecil;
using AstTypeReference = ICSharpCode.NRefactory.Ast.TypeReference;
using CecilTypeReference = Mono.Cecil.TypeReference;

namespace Joddes.CS {
    public static class GenericsHelper {

        public static string GetScriptName(MethodDeclaration method) {
            return GetScriptName(method.Name, method.Templates.Count);
        }

        public static string GetScriptName(MemberReferenceExpression member) {
            return GetScriptName(member.MemberName, member.TypeArguments.Count);
        }

        public static string GetScriptName(MethodDefinition method) {
            return GetScriptName(method.Name, method.GenericParameters.Count);
        }

        public static string GetScriptName (TypeDeclaration type)
        {
        	return GetScriptName (type.Name, type.Templates.Count);
        }
		
		public static string GetScriptName (DelegateDeclaration type)
		{
			return GetScriptName (type.Name, type.Templates.Count);
		}

        public static string GetScriptName (AstTypeReference type)
        {
        	var result = GetScriptName (type.Type, type.GenericTypes.Count);
        	var innerType = type as InnerClassTypeReference;
        	if (innerType != null)
        		result = GetScriptName (innerType.BaseType) + "." + result;
        	return result;
        }
		
		public static string GetScriptName (JsTypeInfo type)
		{
			return ReplaceSpecialChars (type.Name);
		}

        public static string GetScriptFullName (TypeDefinition type)
        {
        	if (type == null) {
        		Console.WriteLine ("TypeDefinition is null");
        		return "Object";
        	}
        	return ReplaceSpecialChars (type.FullName);
        }
		
		public static string GetScriptFullName (string fullName)
		{
			return ReplaceSpecialChars (fullName);
		}

        public static string GetScriptFullName (CecilTypeReference type)
        {
        	if (type == null) {
        		Console.WriteLine ("CecilTypeReference is null");
        		return "Object";
			}
            StringBuilder builder = new StringBuilder(type.Namespace);
            if(builder.Length > 0)
                builder.Append('.');
            builder.Append(ReplaceSpecialChars(type.Name));
            return builder.ToString();
        }

        public static string GetTypeMapKey(TypeDefinition type) {
            return GetScriptFullName(type);
        }

        public static string GetTypeMapKey(JsTypeInfo info) {
            return info.FullName;
        }

        public static string GetTypeMapKey (CecilTypeReference type)
        {
        	return GetScriptFullName (type);
        }
			
        static string GetScriptName (string baseName, int paramCount)
        {
        	//return GetPostfixedName(baseName, paramCount, "$");
        	return baseName;
        }
		

        static string ReplaceSpecialChars (string name)
        {
            // TODO: Remove all signs of generics name
            return name.Replace ("`1<T>", "").Replace ("`1", "").Replace("`2", "").Replace('/', '.');
        }

        private static string GetPostfixedName(string baseName, int paramCount, string separator) {
            if(paramCount < 1)
                return baseName;
            return baseName + separator + paramCount;
        }

    }
}
