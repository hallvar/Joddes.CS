using System.Collections;
using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
	public class ReadOnlyCollection<T> : IEnumerable, ICollection, IList, ICollection<T>, IList<T>, IEnumerable<T>
	{
		public ReadOnlyCollection (IList<T> list)
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
				throw new Exception ();
			}
			
			set { }
		}
		
		public void Add (T item)
		{
		}
		
		int System.Collections.IList.Add (object item)
		{
			return 0;
		}
		
		bool System.Collections.IList.Contains (object item)
		{
			return false;
		}
		
		int System.Collections.IList.IndexOf (object item)
		{
			return -1;
		}
		
		void System.Collections.IList.Insert (int index, object item)
		{
		
		}
		
		void System.Collections.IList.Remove (object item)
		{
		}
		
		object System.Collections.IList.this[int index] {
			get {
				throw new Exception ();
			}
			
			set {
				
			}
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
			get { return 0; }
		}

		public bool IsReadOnly {
			get { return true; }
		}

		public void CopyTo (Array array, int index)
		{
		}

		public bool IsSynchronized {
			get { return true; }
		}

		public object SyncRoot {
			get { return new object (); }
		}

		public IEnumerator<T> GetEnumerator ()
		{
			return null;
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return null;
		}
		
		public bool IsFixedSize {
			get {
				return false;
			}
		}
	}
}