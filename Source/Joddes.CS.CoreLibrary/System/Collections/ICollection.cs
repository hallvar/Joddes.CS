using System;

namespace System.Collections
{
	public interface ICollection
	{
		void CopyTo (Array array, int index);
		int Count { get; }
		bool IsSynchronized { get; }
		object SyncRoot { get; }
	}

    public interface ICollection<T> : ICollection {
        
    }
}