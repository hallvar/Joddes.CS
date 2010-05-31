using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Joddes.CS.Console {
    class Program {        
        static void Main(string[] args) {
            PrintInfo();

            var options = new List<string>(2);
            var paths = new List<string>(2);
            foreach(var arg in args) {
                if(arg.StartsWith("/") || arg.StartsWith("-"))
                    options.Add(arg.Substring(1).ToLower());
                else
                    paths.Add(arg);
            }
            var projectLocation = paths[0];

            try {
                var p = new JsTranslator(projectLocation, System.Console.Out);
                p.ForceBuild = options.Contains("build");
				p.Translate();
            } catch(Exception e) {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Error: {0}", e.Message);
				System.Console.WriteLine("Stacktrace: {0}", e.StackTrace);
                System.Console.ResetColor();
            }
        }

        static void PrintInfo ()
        {
            System.Console.WriteLine ("Joddes.CS command line utility");
            System.Console.WriteLine();
        }

        static void PrintUsage ()
        {
            System.Console.WriteLine ("Usage:");
            System.Console.WriteLine("  {0} path\\to\\project.csproj [path\\to\\output.js] [-nobuild]", Path.GetFileName(Environment.GetCommandLineArgs()[0]));
        }
    }
}