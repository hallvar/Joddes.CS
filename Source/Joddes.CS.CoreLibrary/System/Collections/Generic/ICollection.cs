namespace System.Collections.Generic
{
	public interface ICollections<T> : ICollection
	{
		void Add(T item);
		void Clear();
		bool Contains(T item);
		void CopyTo(T[] array, int arrayIndex);
		bool Remove(T item);
		bool IsReadOnly { get; }
	}
}