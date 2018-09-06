using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public class Triple<TKey, TValue, TData>
    {
        public Triple() { }
        public Triple(TKey key, TValue value, TData extra)
        {
            Key = key;
            ExtraData = extra;
            Value = value;
        }
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public TData ExtraData { get; set; }
    }

    public class TripleList<TKey, TValue, TData> : List<Triple<TKey, TValue, TData>>
    {
        public void Add(TKey key, TValue value, TData extra)
        {
            Add(new Triple<TKey, TValue, TData>(key, value, extra));
        }
    }
}