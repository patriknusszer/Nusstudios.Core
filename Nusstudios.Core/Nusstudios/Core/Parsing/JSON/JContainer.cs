using System;
using Nusstudios.Core.Mapping;
using System.Collections.Generic;
using Nusstudios.Core.ManagedTypes;
using Nusstudios.Core.UnmanagedTypes;
using System.Text.RegularExpressions;
using Nusstudios.Core.Mapping.Collections;
using Nusstudios.Core.Mapping.DynamicObject;
using System.Collections;
using Nusstudios.Core.Mapping.Collections.Generic;

namespace Nusstudios.Core.Parsing.JSON
{
    public abstract class JContainer : JValue, IEnumerable<KeyValuePair<object, JValue>>
    {
        public int Count => ((Container)content).Count;

        new public IEnumerator<KeyValuePair<object, JValue>> GetEnumerator()
        {
            if (this is JObject jo) return (IEnumerator<KeyValuePair<object, JValue>>)(jo).GetEnumerator();
            else return (IEnumerator<KeyValuePair<object, JValue>>)((JArray)this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal object content;
        internal string sep = ".";

        protected bool validPath(string path)
        {
            string[] arr = new string[6];
            arr[0] = @"^(?:(?:(?:\{(?<strongokey>.+?)(?=\}(?!\\";
            arr[1] = @"))\})|(?:\[(?<akey>\d+?)(?=\](?!\\\.))\])|(?:(?<weakokey>[^\[\{\(";
            arr[2] = @"].*?)(?=(?:(?<!\\|\]|\}|\))";
            arr[3] = @")|(?:(?<!\]|\}|\)|";
            arr[4] = @")$))))";
            arr[5] = @"?)+$";
            string rgxstr = String.Join(Regex.Escape(sep), arr);
            Regex rgx = new Regex(rgxstr);
            Match mtch = rgx.Match(path);
            return mtch.Success;
        }

        public void RecursiveIterate(Action<string, JValue> callback)
        {
            CSOCore.Iterate("", (Container)content, (path, node) =>
            {
                if (node is StringContainer sc) callback(path, JObject.CreateFromRef(sc));
                else if (node is ArrayContainer ac) callback(path, JArray.CreateFromRef(ac));
                else callback(path, (JValue)node);
            });
        }

        public bool Exists(string path) => CSOCore.Exists((Container)content, path, sep);
        public void CopyOnUpdate(string path, JValue value) => CSOCore.Update(ref content, path, sep, value.DeepClone());

        public void Update(string path, JValue value)
        {
            if (!validPath(path)) throw new Exception("Illegal path");

            if (value is JContainer jc) CSOCore.Update(ref content, path, sep, (jc).content);
            else CSOCore.Update(ref content, path, sep, value);
        }

        public void Del(string path)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            CSOCore.Delete(content, path, sep);
        }

        public JValue QueryCopy(string path)
        {
            string foundPath;
            JValue v = QueryCopy(path, out foundPath);
            if (new CSOPath(foundPath).EqualsInRawAndType((CSOPath)path)) return v;
            else throw new Exception("Path not found");
        }

        public JValue QueryCopy(string path, out string foundPath)
        {
            object cpy = CSOCore.QueryRef(content, path, out foundPath, sep).DeepClone();

            if (cpy is JLeaf jl) return jl;
            else
            {
                if (cpy is StringContainer sc) return JObject.CreateFromRef(sc);
                else return JArray.CreateFromRef((ArrayContainer)cpy);
            }
        }

        public JValue Query(string path)
        {
            string foundPath;
            JValue v = Query(path, out foundPath);
            if (new CSOPath(foundPath).EqualsInRawAndType((CSOPath)path)) return v;
            else throw new Exception("Path not found");
        }

        public JValue Query(string path, out string foundPath)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            object ptr = CSOCore.QueryRef(content, path, out foundPath, sep);

            if (ptr is JLeaf jl) return jl;
            else
            {
                if (ptr is StringContainer sc) return JObject.CreateFromRef(sc);
                else return JArray.CreateFromRef((ArrayContainer)ptr);
            }
        }

        public ref float QueryFloatAlias(string path) => ref QueryFloatAlias(path, null);
        public ref double QueryDoubleAlias(string path) => ref QueryDoubleAlias(path, null);
        public ref decimal QueryDecimalAlias(string path) => ref QueryDecimalAlias(path, null);
        public ref BigRational QueryBigRationalAlias(string path) => ref QueryBigRationalAlias(path, null);
        public ref short QueryInt16Alias(string path) => ref QueryInt16Alias(path, null);
        public ref int QueryInt32Alias(string path) => ref QueryInt32Alias(path, null);
        public ref long QueryInt64Alias(string path) => ref QueryInt64Alias(path, null);
        public ref ushort QueryUInt16Alias(string path) => ref QueryUInt16Alias(path, null);
        public ref uint QueryUInt32Alias(string path) => ref QueryUInt32Alias(path, null);
        public ref ulong QueryUInt64Alias(string path) => ref QueryUInt64Alias(path, null);

        public ref float QueryFloatAlias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedFloat)v).Alias;
        }

        public ref double QueryDoubleAlias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedDouble)v).Alias;
        }

        public ref decimal QueryDecimalAlias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedDecimal)v).Alias;
        }

        public ref BigRational QueryBigRationalAlias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedBigRational)v).Alias;
        }

        public ref string QueryStringAlias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedString)v).Alias;
        }

        public ref short QueryInt16Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt16)v).Alias;
        }

        public ref int QueryInt32Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt32)v).Alias;
        }

        public ref long QueryInt64Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt64)v).Alias;
        }

        public ref ushort QueryUInt16Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt16)v).Alias;
        }

        public ref uint QueryUInt32Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt32)v).Alias;
        }

        public ref ulong QueryUInt64Alias(string path, Action<string> callback)
        {
            if (!validPath(path)) throw new Exception("Illegal path");
            string foundPath;
            JValue v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt64)v).Alias;
        }
    }
}
