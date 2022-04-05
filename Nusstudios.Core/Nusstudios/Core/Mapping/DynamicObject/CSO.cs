using System;
using Nusstudios.Core.ManagedTypes;
using Nusstudios.Core.UnmanagedTypes;
using Nusstudios.Core.Mapping.Collections;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    class CSO
    {
        Container _map;
        string path_sep;

        public CSO(Container inm, string path_separator)
        {
            _map = inm;
            path_sep = path_separator;
        }

        public bool CopyOnUpdate(string path, Object value) => CSOCore.Update(ref _map, path, path_sep, value.DeepClone());
        public bool Update(string path, Object value) => CSOCore.Update(ref _map, path, path_sep, value);
        public bool Delete(string path) => CSOCore.Delete(_map, path, path_sep);

        public Object Query(string path, out string found_path)
        {
            Object a = CSOCore.QueryRef(_map, path, out found_path, path_sep);

            if (a is Container c) return new CSOPtr(c, path_sep);
            else return a;
        }

        public Object QueryCopy(string path, out string foundPath)
        {
            Object a = CSOCore.QueryRef(_map, path, out foundPath, path_sep);

            if (a is Container c) return new CSO(c, path_sep); 
            else return a.DeepClone();
        }

        public ref float QueryFloatAlias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedFloat)v).Alias;
        }

        public ref double QueryDoubleAlias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedDouble)v).Alias;
        }

        public ref decimal QueryDecimalAlias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedDecimal)v).Alias;
        }

        public ref BigRational QueryBigRationalAlias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedBigRational)v).Alias;
        }

        public ref string QueryStringAlias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedString)v).Alias;
        }

        public ref short QueryInt16Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt16)v).Alias;
        }

        public ref int QueryInt32Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt32)v).Alias;
        }

        public ref long QueryInt64Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedInt64)v).Alias;
        }

        public ref ushort QueryUInt16Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt16)v).Alias;
        }

        public ref uint QueryUInt32Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt32)v).Alias;
        }

        public ref ulong QueryUInt64Alias(string path, Action<string> callback)
        {
            string foundPath;
            object v = Query(path, out foundPath);
            callback?.Invoke(foundPath);
            return ref ((ManagedUInt64)v).Alias;
        }
    }
}
