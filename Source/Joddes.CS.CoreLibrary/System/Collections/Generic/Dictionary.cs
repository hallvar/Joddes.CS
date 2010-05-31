using System;
namespace System.Collections.Generic
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public Dictionary (int capacity)
        {
        }

        public TValue this[TKey key] {
            get {
                throw new NotSupportedException();
            }

            set {

            }
        }

        public void Add (TKey key, TValue value)
        {
            throw new System.NotImplementedException ();
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
                throw new System.NotImplementedException ();
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
                throw new System.NotImplementedException ();
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
