namespace System.IO
{
    public class FileInfo
    {
        string path;

        public FileInfo (string path)
        {
            this.path = path;
        }

        public string Name {
            get {
                return path;
            }
        }
    }
}