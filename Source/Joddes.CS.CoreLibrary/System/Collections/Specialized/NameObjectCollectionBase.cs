using System;
using System.Collections.Generic;
namespace System.Collections.Specialized
{
    public abstract class NameObjectCollectionBase : IEnumerable, ICollection
    {

        public int Count {
            get; private set;
        }

        public IEnumerator GetEnumerator ()
        {
            throw new System.NotImplementedException();
        }

        public NameObjectCollectionBase ()
        {
        }

        public void CopyTo (Array array, int index)
        {
            throw new System.NotImplementedException();
        }
        
        
        public bool IsSynchronized {
            get {
                return false;
                //throw new System.NotImplementedException();
            }
        }
        
        
        public object SyncRoot {
            get {
                return false;
                //throw new System.NotImplementedException();
            }
        }
    }
}