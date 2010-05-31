using System;

namespace Jsm.Html5
{
    [Hidden, Native("XMLDocument")]
    public class XmlDocument
    {
        public XmlDocument ()
        {
        }

        [Native("querySelector")]
        public Element QuerySelector (string selector)
        {
            throw new NotSupportedException ();
        }

        [Native("querySelectorAll")]
        public Element[] QuerySelectorAll (string selector)
        {
            throw new NotSupportedException ();
        }
    }
}