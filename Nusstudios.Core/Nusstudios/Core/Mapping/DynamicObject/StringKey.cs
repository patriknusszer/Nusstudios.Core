using System;
using System.Text.RegularExpressions;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    public class StringKey : Key
    {
        public static StringKey[] ToArray(string[] arr)
        {
            StringKey[] keys = new StringKey[arr.Length];
            for (int i = -1; ++i < arr.Length;) keys[i] = (StringKey)arr[i];
            return keys;
        }

        public static explicit operator StringKey(string op) => new StringKey(op);
        public string key;
        private bool boxingRequired = false;

        public StringKey(string key) : this(key, ".") { }
        public StringKey(string key, string path_sep)
        {
            this.path_sep = path_sep;
            this.key = key;
            string[] arr = new string[2];
            arr[0] = @"^(?:[^\[\{\(].*?(?:(?<!\]|\}|\)|";
            arr[1] = @")$))";
            string rgxstr = String.Join(Regex.Escape(path_sep), arr);
            Regex rgx = new Regex(rgxstr);
            if (!rgx.Match(key).Success) boxingRequired = true;
        }

        public override string ToComponent() => ToComponent(boxingRequired);
        public override Key Convert(string path_sep) => new StringKey(key, base.path_sep);
        public string ToComponent(bool forceBoxing) => CSOPath.escapePath_Sep(boxingRequired || forceBoxing ? "{" + key + "}" : key, path_sep);
        public override T ThrowOrGetRawKey<T>() => typeof(T) == typeof(String) ? (T)(object)key : throw new Exception();

        public override bool EqualsInRawAndType(Key k)
        {
            if (k == this) return true;
            else if (k is StringKey okey) return ((StringKey)k).key == key;
            else return false;
        }
    }
}
