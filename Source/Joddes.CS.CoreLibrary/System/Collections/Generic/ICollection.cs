namespace System.Collections.Generic
{
	public interface ICollection<T> : ICollection, IEnumerable<T>
	{
		void Add(T item);
		void Clear();
		bool Contains(T item);
		void CopyTo(T[] array, int arrayIndex);
		bool Remove(T item);
		bool IsReadOnly { get; }
	}
}