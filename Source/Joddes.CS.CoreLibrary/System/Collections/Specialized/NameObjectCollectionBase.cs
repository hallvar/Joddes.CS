using System;
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
                throw new System.NotImplementedException();
            }
        }
        
        
        public object SyncRoot {
            get {
                throw new System.NotImplementedException();
            }
        }
    }
}