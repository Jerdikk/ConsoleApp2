using System.Collections.Generic;

namespace ConsoleApp2
{
    public class Map<TKey, TValue>
    {
        private List<MapValue<TKey, TValue>> _values;

        public Map()
        {
            _values = new List<MapValue<TKey, TValue>>();
        }

        public Map(Map<TKey, TValue> var3)
        {
            _values = new List<MapValue<TKey, TValue>>();
            foreach (MapValue<TKey, TValue> t1 in var3)
            {
                this.put(t1.First(), t1.Second());
            }
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }


        public TValue this[TKey i]
        {
            get
            {
                foreach (MapValue<TKey, TValue> kv in _values)
                {
                    if (kv.First().Equals(i))
                    {
                        return kv.Second();
                    }
                }
                return default(TValue);
            }
            set
            {
                foreach (MapValue<TKey, TValue> kv in _values)
                {
                    if (kv.First().Equals(i))
                    {
                        kv.SetValue(value);
                        return;
                    }
                }

                _values.Add(new MapValue<TKey, TValue>(i, value));
            }
        }
        public void Add(MapValue<TKey, TValue> value)
        {
            _values.Add(value);
        }
        public void Clear()
        {
            _values.Clear();
        }
        public void Remove(MapValue<TKey, TValue> value)
        {
            _values.Remove(value);
        }

        public void Remove(TKey _key)
        {
            foreach (MapValue<TKey, TValue> kv in _values)
            {
                if (kv.First().Equals(_key))
                {
                    _values.Remove(kv);
                    return;
                }
            }
        }


        public IEnumerator<MapValue<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i <= _values.Count; i++)
            {
                if (i == _values.Count) yield break; // Выход из итератора, если закончится алфавит
                yield return _values[i];
            }
        }

        internal TValue get(TKey i)
        {
            foreach (MapValue<TKey, TValue> kv in _values)
            {
                if (kv.First().Equals(i))
                {
                    return kv.Second();
                }
            }
            return default(TValue);
        }

        internal void put(TKey i, TValue value)
        {
            foreach (MapValue<TKey, TValue> kv in _values)
            {
                if (kv.First().Equals(i))
                {
                    kv.SetValue(value);
                    return;
                }
            }

            MapValue<TKey, TValue> mapValue = new MapValue<TKey, TValue>(i, value);
            _values.Add(mapValue);

        }

        public bool Contains(TKey key)
        {
            foreach (MapValue<TKey, TValue> kv in _values)
            {
                if (kv.First().Equals(key)) return true;
            }
            return false;
        }

        public List<TValue> GetValues()
        {
            List<TValue> values = new List<TValue>();
            foreach (MapValue<TKey, TValue> kv in _values)
                values.Add(kv.Second());
            return values;
        }

        public List<TKey> GetKeys()
        {
            List<TKey> values = new List<TKey>();
            foreach (MapValue<TKey, TValue> kv in _values)
                values.Add(kv.First());
            return values;
        }

        internal void Add(TKey i, TValue value)
        {
            this.put(i, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {

            foreach (MapValue<TKey, TValue> kv in _values)
            {
                if (kv.First().Equals(key))
                {
                    value = kv.Second();
                    return true;

                }
            }
            value = default(TValue);
            return false;
        }

        public bool ContainsKey(TKey _key)
        {
            return Contains(_key);
        }

        internal bool isEmpty()
        {
            return (_values.Count == 0);
        }
    }
}