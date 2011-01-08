using System;

namespace Joddes.CS.Html5
{
    [Hidden, Native("Node")]
    public class Node
    {
        [Native("nodeName")]
        public string NodeName {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("nodeValue")]
        public string NodeValue {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("nodeType")]
        public short NodeType {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("textContent")]
        public string TextContent {
            get {
                throw new NotSupportedException();
            }
        }

        [Native("parentNode")]
        public Node ParentNode {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("childNodes")]
        public NodeList ChildNodes {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("firstChild")]
        public Node FirstChild {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("lastChild")]
        public Node LastChild {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("previousSibling")]
        public Node PreviousSibling {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("nextSibling")]
        public Node NextSiblng {
            get {
                throw new NotSupportedException ();
            }
        }

        [Native("attributes")]
        public NamedNodeMap Attributes {
            get {
                throw new NotSupportedException ();
            }
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