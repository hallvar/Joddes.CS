using System;
namespace System
{
    [Hidden]
    public class ObsoleteAttribute : Attribute
    {
        public ObsoleteAttribute ()
        {
        }

        public ObsoleteAttribute (string message)
        {
            throw new NotSupportedException();
        }

        public ObsoleteAttribute (string message, bool error)
        {
            throw new NotSupportedException ();
        }

        public string Message {
            get {
                throw new NotSupportedException ();
            }
        }

        public bool IsError {
            get {
                throw new NotSupportedException ();
            }
        }
    }
}