using System;
using System.IO;
using System.Linq;
using MonoDevelop.Ide.Tasks;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.Core;

namespace MonoDevelop.Joddes.CS
{
    public class JsTranslator : ProjectServiceExtension
    {
        public JsTranslator ()
        {
            MonoDevelop.Core.LoggingService.LogError ("*******Here4");

            MonoDevelop.Ide.IdeApp.ProjectOperations.EndBuild += (object sender, BuildEventArgs args) =>
            {
                var op = (MonoDevelop.Ide.ProjectOperations)sender;

                MonoDevelop.Core.LoggingService.LogError ("EndBuild: {0} ({1})", op.CurrentSelectedBuildTarget.Name, op.CurrentSelectedBuildTarget);

                var asmproj = op.CurrentSelectedBuildTarget as MonoDevelop.Projects.DotNetAssemblyProject;
                var aspnetproj = op.CurrentSelectedBuildTarget as MonoDevelop.AspNet.AspNetAppProject;

                MonoDevelop.Projects.ProjectReferenceCollection references = null;

                if (asmproj != null)
                {
                    references = asmproj.References;
                }
                if (aspnetproj != null)
                {
                    references = aspnetproj.References;

                }

                var projects = MonoDevelop.Ide.IdeApp.Workspace.GetAllProjects ();

                var configId = MonoDevelop.Ide.IdeApp.Workspace.ActiveConfigurationId;

                string path = null;
                if (aspnetproj != null) {
                    path = op.CurrentSelectedBuildTarget.BaseDirectory.Combine ("bin", "js");
                } else {
                    path = op.CurrentSelectedBuildTarget.BaseDirectory.Combine ("bin", configId, "js");
                }
                var dst = new DirectoryInfo (path);

                foreach (var r in references)
                {

                    if (r.ReferenceType == MonoDevelop.Projects.ReferenceType.Project)
                    {
                        var proj = (from s in MonoDevelop.Ide.IdeApp.Workspace.GetAllSolutions ()
                            from p in s.GetAllProjects ()
                            where p.Name == r.Reference
                            select p).SingleOrDefault ();

                        if (proj == null)
                        {
                            MonoDevelop.Core.LoggingService.LogError ("Couldn't find referance project {0}", r.Reference);
                            continue;
                        }

                        var jsbuilddir = proj.BaseDirectory.Combine ("bin", configId, "js");
                        var src = new DirectoryInfo (jsbuilddir);
                        if (src.Exists) {
                            MonoDevelop.Core.LoggingService.LogError ("Has JS: {0}", proj.Name);

                            CopyAll (src, dst);
                        }
                        //r.OwnerProject.BaseDirectory.FullPath
                    }
                }

                CopyCorLibJsFiles(dst);
            };
        }

        static void CopyCorLibJsFiles (DirectoryInfo dst)
        {
            //var projects = MonoDevelop.Ide.IdeApp.Workspace.GetAllProjects ();

            var configId = MonoDevelop.Ide.IdeApp.Workspace.ActiveConfigurationId;

            var proj = (from s in MonoDevelop.Ide.IdeApp.Workspace.GetAllSolutions ()
                from p in s.GetAllProjects ()
                where p.Name == "Joddes.CS.CoreLibrary"
                select p).SingleOrDefault ();

            if (proj == null) {
                MonoDevelop.Core.LoggingService.LogError ("Couldn't find Joddes.CS.CoreLibrary project");
                return;
            }

            var jsbuilddir = proj.BaseDirectory.Combine ("bin", configId, "js");
            var src = new DirectoryInfo (jsbuilddir);
            if (src.Exists) {
                MonoDevelop.Core.LoggingService.LogError ("Has JS: {0}", proj.Name);

                CopyAll (src, dst);
            }
        }

        public override bool SupportsItem (IBuildTarget item)
        {
            //MonoDevelop.Core.LoggingService.LogError ("*******HEre3");

            return true;
            //return base.SupportsItem (item);
        }
        /*
        protected override SolutionEntityItem LoadSolutionItem (IProgressMonitor monitor, string fileName)
        {
            MonoDevelop.Core.LoggingService.LogError ("*******Here2");
            return base.LoadSolutionItem (monitor, fileName);
        }*/

        protected override BuildResult Build (IProgressMonitor monitor, SolutionEntityItem item, ConfigurationSelector configuration)
        {
            MonoDevelop.Core.LoggingService.LogError ("*******Here");
            var result = base.Build (monitor, item, configuration);

            var el = IdeApp.Workbench.GetPad<MonoDevelop.Ide.Gui.Pads.ErrorListPad> ();
            var errPadContent = (MonoDevelop.Ide.Gui.Pads.ErrorListPad)el.Content;

            errPadContent.AddTask (new Task ("testfilepath", "test desc", 0, 0, TaskSeverity.Error));
            errPadContent.RedrawContent ();
            el.BringToFront ();

            return result;
        }

        static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
    
            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                MonoDevelop.Core.LoggingService.LogInfo(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }
    
            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}