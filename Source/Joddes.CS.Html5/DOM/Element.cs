using System;

namespace Joddes.CS.Html5
{
    [Hidden, Native("Element")]
    public class Element : Node
    {
        [Native("tagName")]
        public string TagName {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("getAttribute")]
        public string GetAttribute (string name)
        {
            throw new NotSupportedException();
        }
    }
}