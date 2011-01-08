using System.Collections.Generic;

namespace System.Collections.Specialized
{
    public class NameValueCollection : NameObjectCollectionBase
    {
        protected Dictionary<string, string> collection = new Dictionary<string, string> ();

        public NameValueCollection ()
        {
        }

        public string this[string name] {
            get {
                return this.collection[name];
            }

            set {
                this.collection[name] = value;
            }
        }
        /*
        public string this[int index] {
            get {
                int i = 0;
                foreach (var v in this.collection.Values)
                {
                    if (i++ == index) {
                        return v;
                    }
                }
                return null;
            }

            set {
                var k = this.GetKey (index);
                this.collection[k] = value;
            }
        }*/

        public new int Count {
            get { return this.collection.Count; }
        }

        public string GetKey (int index)
        {
            int i = 0;
            foreach (var k in this.collection.Keys) {
                if (i++ == index) {
                    return k;
                }
            }
            return null;
        }
    }
}