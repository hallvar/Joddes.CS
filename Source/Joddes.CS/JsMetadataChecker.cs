using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Joddes.CS {
    public static class JsMetadataChecker {

        static bool CanIgnoreType(TypeDefinition type) {
            if(type.IsInterface)
                return true;
            if(HasHiddenAttribute(type))
                return true;
            if(type.BaseType != null && type.BaseType.FullName == "System.MulticastDelegate")
                return true;
            return false;
        }

        public static void CheckType (TypeDefinition type)
        {
        	if (CanIgnoreType (type))
        		return;

			if (type.IsNested) {
        		//JsmException.Throw ("Nested types are not supported: {0}", type);
        		Console.WriteLine ("Nested types are not supported: {0}", type);
			}
            
			//if(type.HasEvents)
            //    throw new ApplicationException(string.Format("Events are not supported: {0}", type));
            //if(type.HasProperties)
            //    JsmException.Throw("Properties are not supported: {0}, you may use methods instead", type);
            if(type.IsValueType)
                Console.WriteLine("Struct types not supported, use classes instead: {0}", type);

            CheckConstructors(type);
            CheckFields(type);
            CheckMethods(type);
        }

        public static bool HasHiddenAttribute (TypeDefinition type)
        {
        	if (type == null) {
        		Console.WriteLine ("HasHiddenAttribute: type is null");
        		return false;
			}
            foreach(CustomAttribute a in type.CustomAttributes) {
                if(a.Constructor.DeclaringType.FullName == "HiddenAttribute")
                    return true;
            }
            return false;
        }

        public static string GetNativeInlineImpl(MethodDefinition method) {
            foreach(CustomAttribute a in method.CustomAttributes) {
                if(a.Constructor.DeclaringType.FullName == "NativeInlineAttribute")
                    return (string)a.ConstructorArguments[0].Value;
            }
            return null;
        }

        public static string GetCustomTypeName (TypeDefinition type)
        {
            foreach (CustomAttribute a in type.CustomAttributes) {
                if (a.Constructor.DeclaringType.FullName == "CustomTypeNameAttribute")
                    return (string)a.ConstructorArguments[0].Value;
            }
            return null;
        }

        public static string GetCustomConstructor (TypeDefinition type)
        {
            foreach (CustomAttribute a in type.CustomAttributes) {
                if (a.Constructor.DeclaringType.FullName == "CustomConstructorAttribute")
                    return (string)a.ConstructorArguments[0].Value;
            }
            return null;
        }

        static void CheckConstructors (TypeDefinition type)
        {
            /*bool found = false;
            foreach (MethodDefinition ctor in type.Methods) {
                if (!ctor.IsConstructor)
                {
                    continue;
                }
                if(ctor.IsStatic)
                    continue;
                CheckMethodArguments(ctor);
                if(found)
                    throw new ApplicationException(string.Format("Type {0} must have the only instance constructor", type));
                found = true;
            }*/
        }

        static void CheckFields(TypeDefinition type) {
        }

        static void CheckMethods (TypeDefinition type)
        {
        	foreach (MethodDefinition method in type.Methods) {
        		if (method.Name.Contains (".")) {
        			Console.WriteLine ("Explicit interface implementations are not supported: {0}", method);
					//JsmException.Throw ("Explicit interface implementations are not supported: {0}", method);
				}
                    
                CheckMethodArguments(method);
                if(!method.IsStatic && !String.IsNullOrEmpty(GetNativeInlineImpl(method)))
                    throw new ApplicationException(string.Format("Native inline implementation in only supported for static methods", method));
            }
        }

        static void CheckMethodArguments (MethodDefinition method)
        {
            foreach (ParameterDefinition param in method.Parameters) {
                if (param.ParameterType is ByReferenceType)
                    throw new ApplicationException (string.Format ("Reference parameters are not supported: {0}", method));
                foreach (CustomAttribute attr in param.CustomAttributes) {
                    if (attr.Constructor.DeclaringType.FullName == "System.ParamArrayAttribute")
                        throw new ApplicationException (string.Format ("Param arrays are not supported: {0}", method));
                }
            }
        }


        public static void CheckDuplicateNames(IDictionary<string, TypeDefinition> allTypes) {
            var parents = GetParentTypes(allTypes);
            foreach(var name in allTypes.Keys) {
                if(parents.Contains(name))
                    continue;
                CheckDuplicateNames(allTypes, allTypes[name]);
            }
        }

        static HashSet<string> GetParentTypes(IDictionary<string, TypeDefinition> allTypes) {
            var result = new HashSet<string>();
            foreach(var type in allTypes.Values) {
                if(type.BaseType != null) {
                    string parentName = type.BaseType.FullName;
                    if(!allTypes.ContainsKey(parentName))
                        throw new ApplicationException(string.Format("Unknown type {0}", parentName));
                    if(!result.Contains(parentName))
                        result.Add(parentName);
                }
            }
            return result;
        }

        static void CheckDuplicateNames (IDictionary<string, TypeDefinition> allTypes, TypeDefinition leaf)
        {
            if (CanIgnoreType (leaf))
                return;

            var instanceMethods = new Dictionary<string, MethodDefinition> ();
            var staticMethods = new Dictionary<string, MethodDefinition> ();
            var instanceFields = new Dictionary<string, FieldDefinition> ();
            var signatureTable = new Dictionary<string, string> ();

            while (true) {

                foreach (FieldDefinition field in leaf.Fields) {
                    if (field.IsStatic)
                        continue;
                    if (instanceFields.ContainsKey (field.Name))
                        throw new ApplicationException (string.Format ("All instance fields within an hierarchy must have unique names: {0} and {1}", field, instanceFields[field.Name]));
                    if (instanceMethods.ContainsKey (field.Name))
                        throw new ApplicationException (string.Format ("A field and a method cannot have the same name: {0} and {1}", field, instanceMethods[field.Name]));
                    instanceFields.Add(field.Name, field);
                }

                foreach(MethodDefinition method in leaf.Methods) {
                    string key = GenericsHelper.GetScriptName(method);
                    if(method.IsStatic) {
                        if(!string.IsNullOrEmpty(GetNativeInlineImpl(method)))
                            continue;
                        if(staticMethods.ContainsKey(key))
                            throw new ApplicationException(string.Format("All static methods within an hierarchy must have unique names: {0} and {1}", method, staticMethods[key]));
                        staticMethods.Add(key, method);
                    } else {
                        if(instanceFields.ContainsKey(key))
                            throw new ApplicationException(string.Format("A field and a method cannot have the same name: {0} and {1}", instanceFields[key], method));
                        if(instanceMethods.ContainsKey(key)) {
                            if(!method.IsVirtual || signatureTable[key] != GetMethodSignatureKey(method))
                                throw new ApplicationException(string.Format("Methods with same name must have same signature and be virtual: {0} and {1}", method, instanceMethods[key]));
                        } else {
                            instanceMethods.Add(key, method);
                            signatureTable.Add(key, GetMethodSignatureKey(method));
                        }
                    }
                }

                if(leaf.BaseType == null)
                    break;
                leaf = allTypes[leaf.BaseType.FullName];
            }
        }


        static string GetMethodSignatureKey(MethodDefinition method) {
            var list = new List<string>(method.Parameters.Count);
            foreach(ParameterDefinition p in method.Parameters)
                list.Add(p.ParameterType.FullName);            
            return String.Join("$", list.ToArray());
        }
    }
}
