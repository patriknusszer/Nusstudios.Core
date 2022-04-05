using System;

namespace Nusstudios.Core.Mapping.Collections
{
    public struct MapEntry
    {
        private Object _key;
        private Object _value;

        // Constructs a new DictionaryEnumerator by setting the Key
        // and Value fields appropriately.
        public MapEntry(Object key, Object value)
        {
            _key = key;
            _value = value;
        }

        public Object Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }

        public Object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }
    }
}
