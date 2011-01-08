using System;
namespace System.Collections.Generic
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Joddes.CS.Html5.Object data = new Joddes.CS.Html5.Object();

        public Dictionary ()
        {
        }

        public TValue this[TKey key] {
            get {
                return (TValue)data[key+""];
            }

            set {
                data[key+""] = value;
            }
        }

        public void Add (TKey key, TValue value)
        {
            data[key+""] = value;
        }


        public bool ContainsKey (TKey key)
        {
            throw new System.NotImplementedException ();
        }


        public bool Remove (TKey key)
        {
            throw new System.NotImplementedException ();
        }

        /*
        public void TryGetValue (TKey key, out TValue value)
        {
            throw new NotSupportedException();
        }*/

        public ICollection<TKey> Keys {
            get {
                var keys = Joddes.CS.Html5.Object.keys (data);
                var lst = new List<TKey> ();
                foreach (var k in keys)
                {
                    lst.Add ((TKey)(object)k);
                }

                return lst;
            }
        }


        public ICollection<TValue> Values {
            get {
                throw new System.NotImplementedException ();
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
        {
            throw new System.NotImplementedException ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            throw new System.NotImplementedException ();
        }

        public void Add (KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException ();
        }


        public void Clear ()
        {
            throw new System.NotImplementedException ();
        }


        public bool Contains (KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException ();
        }


        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException ();
        }


        public bool Remove (KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException ();
        }


        public int Count {
            get {
                var keys = Joddes.CS.Html5.Object.keys (this.data);
                return keys.Length;
                //throw new System.NotImplementedException ();
            }
        }


        public bool IsReadOnly {
            get {
                throw new System.NotImplementedException ();
            }
        }

        public void CopyTo (Array array, int index)
        {
            throw new System.NotImplementedException ();
        }


        public bool IsSynchronized {
            get {
                throw new System.NotImplementedException ();
            }
        }


        public object SyncRoot {
            get {
                throw new System.NotImplementedException ();
            }
        }
    }
}
