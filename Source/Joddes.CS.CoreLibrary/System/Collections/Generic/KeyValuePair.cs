using System;
namespace System.Collections.Generic
{
    public class KeyValuePair<TKey, TValue>
    {
        public KeyValuePair (TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
        
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }
    }
}