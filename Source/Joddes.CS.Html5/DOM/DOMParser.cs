using System;

namespace Jsm.Html5
{
    [Hidden, Native("DOMParser")]
    public class DomParser
    {
        public DomParser ()
        {
        }

        [Native("parseFromString")]
        public XmlDocument ParseFromString (string str, string mimeType)
        {
            throw new NotSupportedException();
        }
    }
}