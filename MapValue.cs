using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public class MapValue<TKey, TValue>
    {

        private TKey _key;
        private TValue _value;


        public MapValue()
        {
            _key = default(TKey);
            _value = default(TValue);
        }

        public MapValue(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is MapValue<TKey, TValue> value &&
                   EqualityComparer<TKey>.Default.Equals(_key, value._key) &&
                   EqualityComparer<TValue>.Default.Equals(_value, value._value);
        }

        public TKey First()
        {
            return _key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_key, _value);
        }

        public TValue Second()
        {
            return _value;
        }

        public void SetValue(TValue value)
        {
            _value = value;
        }

    }
}