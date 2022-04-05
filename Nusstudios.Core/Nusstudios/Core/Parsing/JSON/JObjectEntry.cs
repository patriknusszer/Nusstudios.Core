using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Parsing.JSON
{
    public class JObjectEntry : JEntry
    {
        public string Key => (string)key;

        public JObjectEntry(string key, JValue value)
        {
            this.key = key;
            this.value = value;
        }

        public static implicit operator KeyValuePair<string, JValue>(JObjectEntry op) => new KeyValuePair<string, JValue>(op.Key, op.Value);
        public static implicit operator KeyValuePair<string, object>(JObjectEntry op) => new KeyValuePair<string, object>(op.Key, op.Value);
        public static explicit operator JObjectEntry(KeyValuePair<string, object> op) => new JObjectEntry(op.Key, (JValue)op.Value);
    }
}
