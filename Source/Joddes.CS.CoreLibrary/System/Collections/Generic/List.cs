using System.Collections;

namespace System.Collections.Generic
{
	public class List<T> : IEnumerable<T>, IList<T>, ICollection<T>, ICollection
	{
        private object[] data = new object[]{};

		public List ()
		{
		}
		
		public int IndexOf (T item)
		{
			return 0;
		}

		public void Insert (int index, T item)
		{
		}

		public void RemoveAt (int index)
		{
		}
		
		public T this[int index] {
			get {
				return (T)data[index];
			}
			set {
				data[index] = value;
			}
		}
		
		public void Add (T item)
		{
			this[Count] = item;
		}
		
		public void Clear ()
		{
		}
		
		public bool Contains (T item)
		{
			return true;
		}
		
		public void CopyTo (T[] array, int arrayIndex)
		{
			
		}
		
		public bool Remove (T item)
		{
			return true;
		}
		
		public int Count {
		    get {
                return data.Length;
            }
		}
		
		public bool IsReadOnly {
			get {
				return true;
			}
		}
		
		public void CopyTo (Array array, int index)
		{
		}

		public bool IsSynchronized {
			get {
				return true;
			} 
		}

		public object SyncRoot {
			get {
				return new object ();
			} 
		}
		
		public IEnumerator<T> GetEnumerator ()
		{
			return new ListEnumerator<T>(this);
		}
		
		IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return null;
		}

        public T[] ToArray ()
        {
            return (T[])(object)data;
        }
	}
	
	public class ListEnumerator<T> : IEnumerator<T> {
		int currentPosition = 0;
		List<T> lst;
		
		public ListEnumerator (List<T> list)
		{
			lst = list;
		}
		
		public bool MoveNext ()
		{
			return lst.Count > currentPosition++;
		}
		
		public void Reset ()
		{
			currentPosition = 0;
		}
		
		public T Current {
			get {
				return lst[currentPosition-1];
			}
		}
		
		object System.Collections.IEnumerator.Current {
			get {
				return lst[currentPosition];
			}
		}
		
		public void Dispose() {
			
		}
	}
}