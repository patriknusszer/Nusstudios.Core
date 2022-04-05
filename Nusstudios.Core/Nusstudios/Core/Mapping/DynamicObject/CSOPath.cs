using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    public class CSOPath
    {
        List<Key> klst;
        string path_sep = ".";

        public string PathSep
        {
            get
            {
                return path_sep;
            }
            set
            {
                if (value != path_sep)
                {
                    for (int i = -1; ++i < klst.Count;) klst[i] = klst[i].Convert(value);
                    path_sep = value;
                }
            }
        }

        public Key this[int i] => klst[i];
        public int Count => klst.Count;
        public CSOPath() => klst = new List<Key>();
        public CSOPath(CSOPath path) => klst = path.klst;
        public CSOPath(Key k) : this(new Key[] { k }) { }
        public CSOPath(Key[] klst) : this(klst.ToList()) { }

        public CSOPath(List<Key> klst)
        {
            if (klst.Count > 1) for (int i = 0; ++i < klst.Count;) if (klst[i - 1].path_sep != klst[i].path_sep) throw new Exception("Array of keys are not compatible with each other");
            this.klst = klst;
        }

        public CSOPath(string path) : this(path, ".") { }

        public CSOPath(string path, string path_sep)
        {
            Key[] keys;
            if (!tryParsePath(path, path_sep, out keys)) throw new Exception();
            klst = keys.ToList();
        }

        public static CSOPath operator +(CSOPath lhs, CSOPath rhs)
        {
            lhs.PushSafe(rhs);
            return lhs;
        }

        public static explicit operator CSOPath(string op) => new CSOPath(op);
        public static explicit operator CSOPath(Key[] op) => new CSOPath(op);
        public static explicit operator CSOPath(List<Key> op) => new CSOPath(op);

        public static implicit operator CSOPath(Key op) => new CSOPath(op);
        public static implicit operator List<Key>(CSOPath op) => op.klst.DeepClone();
        public static implicit operator Key[](CSOPath op) => op.klst.DeepClone().ToArray();

        public void PushSafe(CSOPath path) => PushSafe(this, path);
        public void InsertSafe(CSOPath path, int index) => InsertSafe(this, path, index);
        public int IndexOf(CSOPath sub) => IndexOf(this, sub);
        public int LastIndexOf(CSOPath sub) => LastIndexOf(this, sub);
        public bool EqualsInRawAndType(CSOPath path) => EqualsInRawAndType(this, path);
        public CSOPath GetRange(int index, int length) => GetRange(this, index, length);
        public bool IsCompatibleWith(CSOPath path) => path_sep == path.path_sep;
        public bool HasIndirection() => HasIndirection(this);
        public Key GetParentName() => GetParentName(this);
        public CSOPath GetParent() => GetParent(this);
        public Key GetLeafName() => GetLeafName(this);
        public CSOPath SubtractFromOrigin(CSOPath subtrahend) => GetRelativePath(this, subtrahend);
        public string ToPathString() => ToPathString(this);

        public static bool HasIndirection(CSOPath path) => path.klst.Count > 1;
        public static Key GetParentName(CSOPath path) => path.Count > 1 ? path[path.Count - 2] : throw new Exception();
        public static CSOPath GetParent(CSOPath path) => path.Count > 1 ? path.GetRange(0, path.Count - 2) : throw new Exception();
        public static Key GetLeafName(CSOPath path) => path.Count != 0 ? path[path.Count - 1] : throw new Exception();
        public static string ToPathString(CSOPath path) => parseKeys(path.klst.ToArray(), path.path_sep);

        public static CSOPath GetRelativePath(CSOPath minuend, CSOPath subtrahend)
        {
            if (minuend.Count < subtrahend.Count) throw new Exception("Minuend can not be shorter than subtrahend");
            else if (minuend.Count == subtrahend.Count) return null;
            else
            {
                for (int i = -1; ++i < subtrahend.Count;) if (!subtrahend[i].EqualsInRawAndType(minuend[i])) throw new Exception("Subtrahend is not a subpath of minuend from its root");
                return (CSOPath)minuend.klst.GetRange(subtrahend.Count, minuend.Count - subtrahend.Count);
            }
        }

        public static CSOPath PushSafe(CSOPath lhs, CSOPath rhs) => InsertSafe(lhs, rhs, lhs.Count);
        public static CSOPath GetRange(CSOPath path, int index, int length) => new CSOPath(path.klst.GetRange(index, length));
        public static int IndexOf(CSOPath path, CSOPath sub) => ArrayUtil.IndexOfSubArray<Key>(path, sub);
        public static int LastIndexOf(CSOPath path, CSOPath sub) => ArrayUtil.LastIndexOfSubArray<Key>(path, sub);

        public static bool EqualsInRawAndType(CSOPath path1, CSOPath path2)
        {
            if (path1.Count != path2.Count) return false;
            for (int i = -1; ++i < path1.Count;) if (!path1[i].EqualsInRawAndType(path2[i])) return false;
            return true;
        }

        public static CSOPath InsertSafe(CSOPath lhs, CSOPath rhs, int index)
        {
            CSOPath dp = rhs.DeepClone();

            if (!lhs.IsCompatibleWith(rhs))
            {
                dp.PathSep = lhs.PathSep;
            }

            lhs.klst.InsertRange(index, dp.klst);
            return lhs;
        }

        public static T ThrowOrConvertKey<T>(Key k)
        {
            if (k is NumericKey || k is ArrayKey)
            {
                if (typeof(T) != typeof(Int32)) throw new Exception();
                else if (k is NumericKey) return (T)(object)(NumericKey)k;
                else return (T)(object)(ArrayKey)k;
            }
            else
            {
                if (typeof(T) != typeof(string)) throw new Exception();
                else return (T)(object)(StringKey)k;
            }
        }

        private static string parseKeys(Key[] keys, string path_sep)
        {
            string path = "";

            for (int i = -1; ++i < keys.Length;)
            {
                Key k;
                if (!keys[i].IsCompatibleWith(path_sep)) k = keys[i].Convert(path_sep);
                else k = keys[i];
                path += k.ToComponent();
                if (i != keys.Length - 1) path += path_sep;
            }

            return path;
        }

        public static string unescapePath_Sep(string key, string path_sep) => Regex.Replace(key, @"\+" + Regex.Escape(path_sep), (mtch) => {
            string ret = path_sep;
            for (int i = -1; ++i < mtch.Groups[0].Value.Length - 2;) path_sep = @"\" + path_sep;
            return ret;
        });

        public static string escapePath_Sep(string key, string path_sep) => Regex.Replace(key, @"\*" + Regex.Escape(path_sep), (mtch) => @"\" + mtch.Groups[0].Value);

        private static bool tryParsePath(string path, string path_sep, out Key[] components)
        {
            components = null;
            string[] arr = new string[7];
            arr[0] = @"^(?:(?:(?:\{(?<strongokey>.+?)(?=\}(?!\\";
            arr[1] = @"))\})|(?:\[(?<akey>\d+?)(?=\](?!\\\.))\])|(?:\((?<nkey>\d+?)(?=\)(?!\\";
            arr[2] = @"))\))|(?:(?<weakokey>[^\[\{\(";
            arr[3] = @"].*?)(?=(?:(?<!\\|\]|\}|\))";
            arr[4] = @")|(?:(?<!\]|\}|\)|";
            arr[5] = @")$))))";
            arr[6] = @"?)+$";
            string rgxstr = String.Join(Regex.Escape(path_sep), arr);
            Regex rgx = new Regex(rgxstr);
            Match mtch = rgx.Match(path);

            if (mtch.Success)
            {
                arr[0] = @"(?:(?:\{(?<strongokey>.+?)(?=\}(?!\\";
                arr[1] = @"))\})|(?:\[(?<akey>\d+?)(?=\](?!\\";
                arr[2] = @"))\])|(?:\((?<nkey>\d+?)(?=\)(?!\\";
                arr[3] = @"))\))|(?:(?<weakokey>[^\[\{\(";
                arr[4] = @"].*?)(?=(?:(?<!\\|\]|\}|\))";
                arr[5] = @")|(?:(?<!\]|\}|\)|";
                arr[6] = @")$))))";
                rgxstr = String.Join(Regex.Escape(path_sep), arr);
                rgx = new Regex(rgxstr);
                MatchCollection mtchcoll = rgx.Matches(path);
                Key[] comps = new Key[mtchcoll.Count];

                for (int i = -1; ++i < comps.Length;)
                {
                    string s;
                    if (mtchcoll[i].Groups["akey"].Success) comps[i] = (ArrayKey)Convert.ToInt32(s = unescapePath_Sep(mtchcoll[i].Groups["akey"].Value, path_sep));
                    else if (mtchcoll[i].Groups["nkey"].Success) comps[i] = (NumericKey)Convert.ToInt32(unescapePath_Sep(mtchcoll[i].Groups["nkey"].Value, path_sep));
                    else if (mtchcoll[i].Groups["weakokey"].Success) comps[i] = new StringKey(unescapePath_Sep(mtchcoll[i].Groups["weakokey"].Value, path_sep), path_sep);
                    else comps[i] = new StringKey(unescapePath_Sep(mtchcoll[i].Groups["strongokey"].Value, path_sep), path_sep);
                }

                components = comps;
                return true;
            }
            else return false;
        }
    }
}
