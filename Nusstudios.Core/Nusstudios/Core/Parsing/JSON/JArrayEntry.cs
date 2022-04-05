using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Parsing.JSON
{
    public class JArrayEntry : JEntry
    {
        public int Key => (int)key;

        public JArrayEntry(int key, JValue value)
        {
            this.key = key;
            this.value = value;
        }

        public static implicit operator KeyValuePair<int, JValue>(JArrayEntry op) => new KeyValuePair<int, JValue>(op.Key, op.Value);
        public static implicit operator KeyValuePair<int, object>(JArrayEntry op) => new KeyValuePair<int, object>(op.Key, op.Value);
        public static explicit operator JArrayEntry(KeyValuePair<int, object> op) => new JArrayEntry(op.Key, (JValue)op.Value);
    }
}
