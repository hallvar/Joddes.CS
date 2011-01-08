namespace System.IO
{
    public class FileInfo
    {
        string path;

        public FileInfo (string path)
        {
            this.path = path;
        }

        public DirectoryInfo Directory {
            get; set;
        }

        public string DirectoryName {
            get {
                if (this.Directory != null)
                {
                    return Directory.Name;
                }
                return null;
            }
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

        public DateTime LastAccessTime {
            get; set;
        }

        public DateTime LastAccessTimeUtc {
            get;set;
        }

        public DateTime LastWriteTime {
            get;set;
        }

        public DateTime LastWriteTimeUtc {
            get;set;
        }

        public DateTime CreationTime {
            get;set;
        }

        public DateTime CreationTimeUtc {
            get;set;
        }

        public string Extension {
            get {
                var i = this.Name.LastIndexOf (".");
                if (i >= 0) {
                    return this.Name.Substring (i + 1);
                }
                return null;
            }
        }

        public bool IsReadOnly {
            get {
                return true;
            }
        }

        public bool Exists {
            get {
                throw new NotSupportedException();
            }
        }
    }
}