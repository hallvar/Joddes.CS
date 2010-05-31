using System.Collections.Generic;

namespace System.Collections.Specialized
{
    public class NameValueCollection : NameObjectCollectionBase
    {
        Dictionary<string, string> collection = new Dictionary<string, string>(8);

        public NameValueCollection ()
        {
        }

        public string this[string name] {
            get {
                return collection[name];
            }

            set {
                collection[name] = value;
            }
        }

        public string this[int index] {
            get {
                int i = 0;
                foreach (var v in collection.Values)
                {
                    if (i++ == index) {
                        return v;
                    }
                }
                return null;
            }

            set {
                var k = this.GetKey (index);
                collection[k] = value;
            }
        }

        public new int Count {
            get {
                return collection.Count;
            }
        }


        public string GetKey (int index)
        {
            int i = 0;
            foreach (var k in collection.Keys) {
                if (i++ == index) {
                    return k;
                }
            }
            return null;
        }
    }
}