namespace System.Collections.Generic
{
    public interface IDictionary<TKey, TValue> : IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        void Add (TKey key, TValue value);
        bool ContainsKey (TKey key);
        bool Remove (TKey key);
        //void TryGetValue (TKey key, out TValue value);
        ICollection<TKey> Keys { get; }
        ICollection<TValue> Values { get; }
    }
}