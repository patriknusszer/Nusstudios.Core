using System;
using System.Collections;
using Nusstudios.Core.Mapping;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Parsing.JSON
{
    public class JObject : JContainer, IEnumerable<KeyValuePair<string, JValue>>
    {
        new public IEnumerator <KeyValuePair<string, JValue>> GetEnumerator() => new JObjectEnumarator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class JObjectEnumarator : IEnumerator<KeyValuePair<string, JValue>>
        {
            IEnumerator<KeyValuePair<string, object>> it;

            public JObjectEnumarator(JObject o) => it = ((StringContainer)o.content).GetEnumerator();

            public KeyValuePair<string, JValue> Current
            {
                get
                {
                    JValue tmp;
                    if (it.Current.Value is JLeaf jl) tmp = jl;
                    else if (it.Current.Value is StringContainer sc) tmp = JObject.CreateFromRef(sc);
                    else tmp = JArray.CreateFromRef((ArrayContainer)it.Current.Value);
                    return new KeyValuePair<string, JValue>(it.Current.Key, tmp);
                }
            }

            object IEnumerator.Current => Current;
            public void Dispose() => it.Dispose();
            public bool MoveNext() => it.MoveNext();
            public void Reset() => it.Reset();
        }

        internal static JObject CreateFromRef(StringContainer sc)
        {
            JObject tmp = new JObject();
            tmp.content = sc;
            return tmp;
        }

        public JObject() => content = new StringContainer();
        public JObject(JObject op) => content = op.content.DeepClone();
        public JObject(string s) => init(s, ".");
        public JObject(string s, string path_sep) => init(s, path_sep);

        private void init(string s, string path_sep)
        {
            if (!JSONCore.IsJObject(JSONCore.GuessElementTypeAt(0, s))) throw new Exception("Provided string is not a valid JSON Object");
            sep = path_sep;
            int i = 0;
            content = JSONCore.ReadObjectAt(ref i, s);
        }

        public override JValue this[object key]
        {
            get
            {
                if (key is string s) return this[s];
                else throw new Exception("Key is required to be a string");

            }
            set
            {
                if (key is string s) this[s] = value;
                else throw new Exception("Key is required to be a string");

            }
        }

        public JValue this[string path]
        {
            get => Query(path);
            set => Update(path, value);
        }

        public void Add(JObjectEntry entry) => ((StringContainer)content).Add(entry.Key, entry.Value);
        public void Add(string key, JValue value) => ((StringContainer)content).Add(key, value);
        public void Remove(string key) => ((StringContainer)content).Remove(key);
    }
}
