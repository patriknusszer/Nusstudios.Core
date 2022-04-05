using System;
using System.Collections;
using System.Collections.Generic;
using Nusstudios.Core.Mapping.Collections;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Parsing.JSON
{
    public class JArray : JContainer, IEnumerable<KeyValuePair<int, JValue>>
    {
        new public IEnumerator<KeyValuePair<int, JValue>> GetEnumerator() => new JArrayEnumarator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class JArrayEnumarator : IEnumerator<KeyValuePair<int, JValue>>
        {
            IEnumerator<KeyValuePair<int, object>> it;

            public JArrayEnumarator(JArray o) => it = ((ArrayContainer)o.content).GetEnumerator();

            public KeyValuePair<int, JValue> Current
            {
                get
                {
                    JValue tmp;
                    if (it.Current.Value is JLeaf jl) tmp = jl;
                    else if (it.Current.Value is StringContainer sc) tmp = JObject.CreateFromRef(sc);
                    else tmp = JArray.CreateFromRef((ArrayContainer)it.Current.Value);
                    return new KeyValuePair<int, JValue>(it.Current.Key, tmp);
                }
            }

            object IEnumerator.Current => Current;
            public void Dispose() => it.Dispose();
            public bool MoveNext() => it.MoveNext();
            public void Reset() => it.Reset();
        }

        internal static JArray CreateFromRef(ArrayContainer ac)
        {
            JArray tmp = new JArray();
            tmp.content = ac;
            return tmp;
        }

        public JArray() => content = new ArrayContainer();
        public JArray(string s) => init(s, ".");
        public JArray(string s, string path_sep) => init(s, path_sep);

        private void init(string s, string path_sep)
        {
            if (!JSONCore.IsJArray(JSONCore.GuessElementTypeAt(0, s))) throw new Exception("Provided string is not a valid JSON Array");
            int i = 0;
            content = JSONCore.ReadArrayAt(ref i, s);
        }

        public override JValue this[object key]
        {
            get
            {
                if (key is String s) return this[s];
                else if (key is Int32 i) return this[i];
                else throw new Exception("Key is required to be an Int32 or String");
            }
            set
            {
                if (key is String s) this[s] = value;
                else if (key is Int32 i) this[i] = value;
                else throw new Exception("Key is required to be an Int32 or String");
            }
        }

        public JValue this[string path]
        {
            get => Query(path);
            set => Update(path, value);
        }

        public JValue this[int key]
        {
            get => Query("[" + key + "]");
            set => Update("[" + key + "]", value);
        }

        public void Add(JValue value) => Add(Count, value);
        public void Add(JArrayEntry entry) => Add(entry.Key, entry.Value);
        public void Remove(int key) => ((ArrayContainer)content).Remove(key);

        public void Add(int key, JValue value)
        {
            if (value is JContainer jc) ((ArrayContainer)content).Add(key, (jc).content);
            else ((ArrayContainer)content).Add(key, value);
        }
    }
}
