namespace System.IO
{
    public class DirectoryInfo
    {
        string path;

        public DirectoryInfo (string path)
        {
            this.path = path;
        }

        public DirectoryInfo[] GetDirectories ()
        {
            return null;
        }

        public FileInfo[] GetFiles ()
        {
            return null;
        }
    }
}