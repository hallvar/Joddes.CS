using System;
using System.Collections.Generic;
using System.IO;

namespace System.Xml.Serialization
{
	public class XmlSerializer
	{
		private Type type;
		
		public XmlSerializer (Type type)
		{
			this.type = type;
		}
		
		public object Deserialize (Stream stream)
		{
		    var ci = type.GetConstructors ()[0];
		    Joddes.CS.Html5.Object obj = ci.Invoke (null) as Joddes.CS.Html5.Object;

            //var xhrStream = stream as Jsm.Html5.Object;
      //if (xhrStream["responseXML"] != null) {
      //}

			var sr = new StreamReader (stream);
		    //Jsm.Html5.Console.Log(stream);
		    var doc = sr.ReadToEnd ();
		    //Jsm.Html5.Console.Log (doc);

            var p = new Joddes.CS.Html5.DomParser ();
            var xdoc = p.ParseFromString (doc, "text/xml");
		 
			//var members = type.GetMembers ();
		    var members = Joddes.CS.Html5.Object.keys (obj.prototype);
			foreach (var m in members) 
			{
				var prop = this.type.GetProperty (m);
                object v = this.internalDeserialize(xdoc.QuerySelectorAll(m));
				prop.SetValue (obj, v, null);
			}
			
			return obj;
		}

        private object internalDeserialize (Joddes.CS.Html5.Element[] nodes)
        {
            var ar = new object[nodes.Length];
            int j = 0;
            foreach (Joddes.CS.Html5.Element n in nodes) {
                var obj = new Joddes.CS.Html5.Object ();

                for (int i = 0; i < n.ChildNodes.Length; i++) {
                    //var cn = n.ChildNodes.Item (i);
                    var cn = n.ChildNodes[i];
                    var el = n.QuerySelector (cn.NodeName);
                    if (el != null) {
                        obj[cn.NodeName] = el.FirstChild.NodeValue;
                    }
                }

                ar[j++] = obj;
            }

            //return (object) (lst.Count == 1 ? lst[0] : lst);
            return ar;
        }
	}
}