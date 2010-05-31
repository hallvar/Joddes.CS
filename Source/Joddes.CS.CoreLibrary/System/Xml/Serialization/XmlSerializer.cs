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
		    Jsm.Html5.Object obj = ci.Invoke (null) as Jsm.Html5.Object;
		 
			var sr = new StreamReader (stream);
		    //Jsm.Html5.Console.Log(stream);
			var doc = sr.ReadToEnd ();
		    //Jsm.Html5.Console.Log (doc);

            var p = new Jsm.Html5.DomParser ();
            var xdoc = p.ParseFromString(doc, "text/xml");
			
			//var members = type.GetMembers ();
			var members = Jsm.Html5.Object.keys (obj.prototype);
			foreach (var m in members) 
			{
				var prop = this.type.GetProperty (m);
                object v = this.internalDeserialize(xdoc.QuerySelectorAll(m));
				prop.SetValue (obj, v, null);
			}
			
			return obj;
		}

        private object internalDeserialize (Jsm.Html5.Element[] nodes)
        {
            var lst = new List<object> ();

            foreach (Jsm.Html5.Element n in nodes) {
                var obj = new Jsm.Html5.Object ();

                for (int i = 0; i < n.ChildNodes.Length; i++) {
                    var cn = n.ChildNodes.Item (i);
                    var el = n.QuerySelector (cn.NodeName);
                    if (el != null) {
                        obj[cn.NodeName] = el.FirstChild.NodeValue;
                    }
                }

                lst.Add (obj);
            }

            //return (object) (lst.Count == 1 ? lst[0] : lst);
            return lst;
        }
	}
}