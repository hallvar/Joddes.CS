using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Mono.Cecil;

namespace Joddes.CS {
    public class JsTranslator {
        protected string Configuration { get;set; }
        protected string Location { get; set; }
        protected string CorlibLocation { get; set; }
        protected TextWriter Log { get; set; }

        protected string AssemblyLocation { get; set; }
        protected IList<string> SourceFiles { get; set; }

        public bool ForceBuild { get; set; }

        public JsTranslator (string location, string corlibLocation, TextWriter log)
        {
            Location = location;
            CorlibLocation = corlibLocation;
            Log = log;
        }

        public void Translate ()
        {
            Configuration = "Debug";
            Log.WriteLine ("Reading project file");
            ReadProjectFile ();
            Log.WriteLine ("Assembly location: {0}", AssemblyLocation);
            Log.WriteLine ("{0} source files", SourceFiles.Count);
            Log.WriteLine ();

            if (ForceBuild || !File.Exists (AssemblyLocation)) {
                Log.WriteLine ("Building");
                BuildProject ();
                Log.WriteLine ();
            }

            Log.WriteLine ("Loading assembly");
            var references = new HashSet<AssemblyDefinition> ();
            var assembly = LoadAssembly (AssemblyLocation, references);
            if (references.Count > 0)
                Log.WriteLine ("{0} referenced assemblies also loaded", references.Count);
            Log.WriteLine ();

            Log.WriteLine ("Adding assembly for corlib: {0}", CorlibLocation);

            if (CorlibLocation != null) {
                var corlib = AssemblyDefinition.ReadAssembly (CorlibLocation);
                Log.WriteLine ("Corlib: {0}", corlib);
                references.Add (corlib);
                Log.WriteLine ();
            }

            Log.WriteLine ("Filling type map");
            var knownTypes = new Dictionary<string, TypeDefinition> ();
            FillTypeMap (assembly, knownTypes);
            foreach (var item in references) {
                //Console.WriteLine("*****Reference: {0}", item.Name);
                FillTypeMap (item, knownTypes);
            }
   
            Log.WriteLine ("Checking type hierarchy");
        	//JsmMetadataChecker.CheckDuplicateNames(knownTypes);

            Log.WriteLine ();
        
            Log.WriteLine ("Processing source files");
        	var inspector = new JsSourceInspector ();
        	var prefix = Path.GetDirectoryName (Location);
        	foreach (var path in SourceFiles) {
        		var p = path.Replace ('\\', Path.DirectorySeparatorChar);
        		Log.Write ("  ");
        		Log.WriteLine (p);
        		inspector.VisitCompilationUnit (ParseFile (Path.Combine (prefix, p)), null);
        	}
        	Log.WriteLine ();
        

			Log.WriteLine ("Emitting Javascript code");
			
			foreach(JsTypeInfo t in inspector.CollectedTypes) {
				//if(t.ClassType != ClassType.Interface) {
					var emitter = new JsEmitter (knownTypes, inspector.CollectedTypes);
					emitter.Emit(t);
					var dir = Path.Combine("js", t.Namespace.Replace('.', Path.DirectorySeparatorChar));
					
					if(!Directory.Exists(dir)) {
						Directory.CreateDirectory(dir);
					}
					var outputLocation = Path.Combine(dir, GenericsHelper.GetScriptName(t)+".js");
					File.WriteAllText (outputLocation, emitter.Output.ToString());
				//}
			}
            //emitter.Emit(inspector.CollectedTypes);
            Log.WriteLine();

            //return emitter.Output.ToString();
        }

        // Project file

        void ReadProjectFile() {
            var doc = new XmlDocument();
            doc.Load(Location);

            var manager = new XmlNamespaceManager(new NameTable());
            manager.AddNamespace("my", "http://schemas.microsoft.com/developer/msbuild/2003");

            AssemblyLocation = Path.Combine(GetOutputPath(doc, manager), GetAssemblyName(doc, manager) + ".dll");
            SourceFiles = GetSourceFiles(doc, manager);
        }

        string GetOutputPath (XmlDocument doc, XmlNamespaceManager manager)
        {
        	var nodes = doc.SelectNodes ("//my:PropertyGroup[contains(@Condition,'"+Configuration+"')]/my:OutputPath", manager);
        	if (nodes.Count != 1)
        		throw new ApplicationException ("Unable to determine output path");
        	var path = nodes[0].InnerText;
        	if (!Path.IsPathRooted (path)) {
				path = path.Replace ("\\", "/");
				path = Path.GetFullPath (Path.Combine (Path.GetDirectoryName (Location), path));
			}
          
			return path;
        }

        IList<string> GetSourceFiles(XmlDocument doc, XmlNamespaceManager manager) {
            var result = new List<string>();
            foreach(XmlNode node in doc.SelectNodes("//my:Compile[@Include]", manager)) {
                result.Add(node.Attributes["Include"].InnerText);
            }
            return result;
        }

        string GetAssemblyName(XmlDocument doc, XmlNamespaceManager manager) {
            var nodes = doc.SelectNodes("//my:AssemblyName", manager);
            if(nodes.Count != 1)
                throw new ApplicationException("Unable to determine assembly name");
            return nodes[0].InnerText;
        }

        // Compiling

        string GetBuildAppName() {
            switch(Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                    return String.Format("{0}\\Microsoft.NET\\Framework\\v3.5\\msbuild", Environment.GetEnvironmentVariable("windir"));
                default:
                    throw new ApplicationException(string.Format("Don't know how to build a project on platform {0}", Environment.OSVersion.Platform));
            }
        }

        string GetBuildAppArguments() {
            switch(Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                    return String.Format(" \"{0}\" /t:Build /p:Configuation=Debug", Location);
                default:
                    throw new NotSupportedException();
            }
        }

        void BuildProject() {            
            var info = new ProcessStartInfo() { 
                FileName = GetBuildAppName(),
                Arguments = GetBuildAppArguments()
            };
            using(var p = Process.Start(info)) {
                p.WaitForExit();
                if(p.ExitCode != 0)
                    throw new ApplicationException("Compiler returned non zero exit code");
            }           
        }

        // Assemblies

        public static AssemblyDefinition LoadAssembly (string location, HashSet<AssemblyDefinition> references)
        {
            if (!File.Exists (location)) {
                return null;
            }
            var ass = AssemblyDefinition.ReadAssembly (location);
            foreach (AssemblyNameReference r in ass.MainModule.AssemblyReferences) {
                if (r.Name == "mscorlib" || r.Name == "System.CoreEx" || r.Name == "System.Reactive") {
					//JsmException.Throw("Assembly references mscorlib: {0}", ass.Name);
					continue;
				}
                var path = Path.Combine(Path.GetDirectoryName(location), r.Name) + ".dll";
                var reference = LoadAssembly(path, references);
                if(reference != null && !references.Contains(reference))
                    references.Add(reference);
            }
            return ass;
        }

        // Types

        static void FillTypeMap (AssemblyDefinition assembly, IDictionary<string, TypeDefinition> types)
        {
            foreach (TypeDefinition type in assembly.MainModule.Types) {
                //Console.WriteLine ("Type: {0} ({1})", type.FullName, assembly.Name);
                var key = GenericsHelper.GetTypeMapKey (type);
                if (types.ContainsKey (key)) {
                    Console.WriteLine ("Key exists in types already: {0} ({1})", type.FullName, assembly.MainModule.Name);
                } else {
                    JsMetadataChecker.CheckType (type);
                    types.Add(key, type);
				}
			}
        }

        // Source code

        static CompilationUnit ParseFile(string location) {
            using(var stream = File.OpenRead(location))
            using(var reader = new StreamReader(stream))
            using(var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, reader)) {
                parser.Parse();
                if(parser.Errors.Count > 0)
                    throw new ApplicationException(string.Format("Error parsing file {0}: {1}", location, parser.Errors.ErrorOutput));
                return parser.CompilationUnit;
            }
        }
    }

}
