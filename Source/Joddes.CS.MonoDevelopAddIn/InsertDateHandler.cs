using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui.Components;

namespace MonoDevelop.Joddes.CS.DateInserter
{
    class InsertDateHandler : CommandHandler
    {
        protected override void Run ()
        {
            MonoDevelop.Core.LoggingService.LogError ("*********CMD");
        }

        protected override void Update (CommandInfo info)
        {
            MonoDevelop.Core.LoggingService.LogError ("*********UPDATE");
            info.Enabled = true;
            //MonoDevelop.Ide.Gui.Document doc = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
            //info.Enabled = doc != null && doc.GetContent<MonoDevelop.Ide.Gui.Content.IEditableTextBuffer> () != null;
         }
    }

    public enum DateInserterCommands
    {
        InsertDate,
    }
}