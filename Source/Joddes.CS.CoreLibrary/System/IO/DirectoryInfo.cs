using Joddes.CS.Html5;

namespace System.IO
{
    public class DirectoryInfo
    {
        string path;

        public DirectoryInfo (string path)
        {
            this.path = path;
        }

        public string FullName {
            get {
                return this.path;
            }
        }

        public string Name {
            get {
                return this.path.Substring(this.path.LastIndexOf("/")+1);
            }
        }

        [Obsolete("Use BeginGetDirectories in CoreLibrary.Extensions instead.", true)]
        public DirectoryInfo[] GetDirectories ()
        {
            throw new NotSupportedException();
        }

        [Obsolete("Use BeginGetFiles in CoreLibrary.Extensions instead.", true)]
        public FileInfo[] GetFiles ()
        {
            throw new NotSupportedException();
        }
    }
}