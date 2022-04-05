using System;
using Nusstudios.Core.Mapping.Collections;
using Nusstudios.Core.ManagedTypes;
using Nusstudios.Core.UnmanagedTypes;
using System.Collections.Generic;
using Nusstudios.Core.Parsing.Unicode;

namespace Nusstudios.Core.Parsing.JSON
{
    public enum ValueType
    {
        Object,
        Array,
        String,
        Number,
        BooleanTrue,
        BooleanFalse,
        Null,
        None
    }

    // strings
    public static class JSONCore
    {
        public static string WriteValue(object value, int indent, JIndentCfg cfg)
        {
            if (value is StringContainer sc) return WriteObject(sc, indent, cfg);
            else if (value is ArrayContainer ac) return WriteArray(ac, indent, cfg);
            else if (value is JLeaf)
            {
                if (value is ManagedNull) return WriteNull();
                else if (value is ManagedNumber mn) return WriteNumber((BigRational)mn);
                else if (value is ManagedString ms) return WriteString(ms);
                else return WriteBoolean((ManagedBoolean)value);
            }
            else throw new Exception("Value is not a proper JSON value");
        }

        public static string WriteObject(StringContainer value, int indent, JIndentCfg cfg)
        {
            string tmp = "{\n";
            if (indent != 0 && cfg.IndentOpenCurlyBrace) tmp = "\n" + StringUtil.PrependChar(tmp, cfg.IndentString, indent);
            IEnumerator<string> keys = value.Keys.GetEnumerator();

            for (int x = -1; ++x < value.Count - 1;)
            {
                keys.MoveNext();
                tmp += StringUtil.PrependChar(WriteMember(keys.Current, value[keys.Current], indent + 1, cfg), cfg.IndentString, indent + 1);
                tmp += ",\n";
                for (int i = -1; ++i < cfg.ObjectSpacing;) tmp += cfg.IndentSpacing ? StringUtil.PrependChar("\n", cfg.IndentString, indent + 1) : "\n";
            }

            if (keys.MoveNext()) tmp += StringUtil.PrependChar(WriteMember(keys.Current, value[keys.Current], indent + 1, cfg), cfg.IndentString, indent + 1);
            return tmp + "\n" + StringUtil.PrependChar("}", cfg.IndentString, indent);
        }

        public static string WriteMember(string key, object value, int indent, JIndentCfg cfg)
        {
            string tmp = "\"" + key + "\"" + cfg.StuffingAfterMemberKey + ":";
            if (!(value is Container && cfg.IndentOpenCurlyBrace)) tmp += cfg.StuffingBeforeMemberValue;
            return tmp + WriteValue(value, indent, cfg);
        }

        public static string WriteArray(ArrayContainer value, int indent, JIndentCfg cfg)
        {
            string tmp = "[\n";
            if (indent != 0 && cfg.IndentOpenCurlyBrace) tmp = "\n" + StringUtil.PrependChar(tmp, cfg.IndentString, indent);

            for (int x = -1; ++x < value.Count - 1;)
            {
                tmp += StringUtil.PrependChar(WriteValue(value[x], indent + 1, cfg), cfg.IndentString, indent + 1);
                tmp += ",\n";
                for (int i = -1; ++i < cfg.ArraySpacing;) tmp += cfg.IndentSpacing ? StringUtil.PrependChar("\n", cfg.IndentString, indent + 1) : "\n";
            }

            if (1 <= value.Count) tmp += StringUtil.PrependChar(WriteValue(value[value.Count - 1], indent + 1, cfg), cfg.IndentString, indent + 1);
            return tmp + "\n" + StringUtil.PrependChar("]", cfg.IndentString, indent);
        }

        // ---------------------------------------------------------------------------------------------------------------------------------------------------

        public static string WriteValue(object value)
        {
            if (value is StringContainer sc) return WriteObject(sc);
            else if (value is ArrayContainer ac) return WriteArray(ac);
            else if (value is JLeaf)
            {
                if (value is ManagedNull) return WriteNull();
                else if (value is ManagedNumber mn) return WriteNumber((BigRational)mn);
                else if (value is ManagedString ms) return WriteString(ms);
                else return WriteBoolean((ManagedBoolean)value);
            }
            else throw new Exception("Value is not a proper JSON value");
        }

        public static string WriteObject(StringContainer value)
        {
            string tmp = "{";
            IEnumerator<string> keys = value.Keys.GetEnumerator();

            for (int x = -1; ++x < value.Count;)
            {
                keys.MoveNext();
                tmp += WriteMember(keys.Current, value[keys.Current]);
                if (x != value.Count - 1) tmp += ",";
            }

            return tmp + "}";
        }

        public static string WriteMember(string key, object value) => "\"" + key + "\":" + WriteValue(value);

        public static string WriteArray(ArrayContainer value)
        {
            string tmp = "[";

            for (int x = -1; ++x < value.Count;)
            {
                tmp += WriteValue(value[x]);
                if (x != value.Count - 1) tmp += ",";
            }

            return tmp += "]";
        }

        public static string WriteNull() => "null";
        public static string WriteString(string value) => "\"" + value + "\"";
        public static string WriteNumber(BigRational value) => value.ToDecimalString(50);
        public static string WriteBoolean(bool value) => value ? "true" : "false";

        public static bool IsJValue(string s) => !GuessElementTypeAt(0, s).Equals(ValueType.None);
        public static bool IsJContainer(string s) => GuessElementTypeAt(0, s).Equals(ValueType.Object) ^ GuessElementTypeAt(0, s).Equals(ValueType.Array);
        public static bool IsJArray(string s) => GuessElementTypeAt(0, s).Equals(ValueType.Array);
        public static bool IsJObject(string s) => GuessElementTypeAt(0, s).Equals(ValueType.Object);
        public static bool IsJLeaf(string s) => !IsJContainer(s);

        public static bool IsJValue(ValueType s) => s.Equals(ValueType.None);
        public static bool IsJContainer(ValueType s) => s.Equals(ValueType.Object) ^ s.Equals(ValueType.Array);
        public static bool IsJArray(ValueType s) => s.Equals(ValueType.Array);
        public static bool IsJObject(ValueType s) => s.Equals(ValueType.Object);
        public static bool IsJLeaf(ValueType s) => !IsJContainer(s);

        public static ValueType GuessValueTypeAt(int i, string s)
        {
            switch (s[i])
            {
                case '{': return ValueType.Object;
                case '[': return ValueType.Array;
                case '\"': return ValueType.String;
                case 't': return ValueType.BooleanTrue;
                case 'f': return ValueType.BooleanFalse;
                case 'n': return ValueType.Null;
                default:
                    if (s[i].Equals('-') ^ Char.IsDigit(s[i])) return ValueType.Number;
                    else return ValueType.None;
            }
        }

        public static StringContainer ReadObjectAt(ref int i, string s)
        {
            if (s.Length - i < 2) throw new Exception();
            if (s[i] != '{') throw new Exception();
            StringContainer obj = new StringContainer();
            i++;

            if (s[i] != '}')
            {
                do
                {
                    obj.Add(ReadMemberAt(ref i, s));

                    if (s[i] != ',') break;
                    else i++;
                }
                while (true);

                if (s[i] != '}') throw new Exception();
            }

            i++;
            return obj;
        }

        public static ArrayContainer ReadArrayAt(ref int i, string s)
        {
            if (s.Length - i < 2) throw new Exception();
            if (s[i] != '[') throw new Exception();
            ArrayContainer obj = new ArrayContainer();
            int index = 0;
            i++;

            if (s[i] != ']')
            {
                do
                {
                    obj.Add(new KeyValuePair<int, object>(index, ReadElementAt(ref i, s)));

                    if (s[i] != ',') break;
                    else
                    {
                        i++;
                        index++;
                    }
                }
                while (true);

                if (s[i] != ']') throw new Exception();
            }

            i++;
            return obj;
        }

        public static KeyValuePair<string, object> ReadMemberAt(ref int i, string s)
        {
            EatStuffing(ref i, s);
            string key = ReadStringAt(ref i, s);
            EatStuffing(ref i, s);

            if (s[i] == ':')
            {
                i++;
                object val = ReadElementAt(ref i, s);
                return new KeyValuePair<string, object>(key, val);
            }
            else throw new Exception();
        }

        public static ManagedString ReadStringAt(ref int i, string s)
        {
            if (s[i] != '\"') throw new Exception();
            string str = "";

            for (int x = ++i; x < s.Length; x++, i++)
            {
                if (0x0020 <= s[x] && s[x] <= 0x10ffff)
                {
                    switch (s[x])
                    {
                        case '\"':
                            i++;
                            return str;
                        case '\\':
                            if (x != s.Length - 1)
                            {
                                x++;
                                switch (s[x])
                                {
                                    case 'b':
                                        str += '\b';
                                        break;
                                    case 'f':
                                        str += '\f';
                                        break;
                                    case 't':
                                        str += '\t';
                                        break;
                                    case '\\':
                                        str += '\\';
                                        break;
                                    case '/':
                                        str += '/';
                                        break;
                                    case 'u':
                                        x++;
                                        if (!(x + 3 <= s.Length - 1))
                                            throw new Exception();
                                        string hex = "";

                                        for (int _x = x; _x < x + 4; _x++)
                                        {
                                            if (Char.IsDigit(s[_x]) ^ ('a' <= s[_x] && s[_x] <= 'f') ^ ('A' <= s[_x] && s[_x] <= 'F'))
                                            {
                                                hex += s[_x];
                                            }
                                            else
                                            {
                                                throw new Exception();
                                            }
                                        }

                                        int intValue = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                                        str += (char)intValue;
                                        break;
                                    default:
                                        throw new Exception();
                                }
                            }

                            break;
                        default:
                            str += s[x];
                            break;
                    }
                }
            }

            throw new Exception();
        }

        private enum Stage
        {
            integer,
            fraction,
            exponent
        }

        public static ManagedBigRational ReadNumberAt(ref int i, string s)
        {
            bool negative = false;

            if (s[i] == '-')
            {
                negative = true;
                i++;
            }

            Stage stg = Stage.integer;
            string integer = "";
            string fraction = "";
            char expsign = '+';
            string exponent = "";

            for (int x = i; x < s.Length; x++, i++)
            {
                if (s[x] == '.')
                {
                    if (String.IsNullOrEmpty(integer)) throw new Exception();
                    if (stg != Stage.integer) throw new Exception();
                    stg = Stage.fraction;
                }
                else if (s[x] == 'e' || s[x] == 'E')
                {
                    if (String.IsNullOrEmpty(integer)) throw new Exception();
                    if (stg == Stage.exponent) throw new Exception();
                    if (x == s.Length - 1) throw new Exception();
                    if (stg == Stage.fraction && String.IsNullOrEmpty(fraction)) throw new Exception();
                    x++;
                    i++;
                    if (s[x] == '+' ^ s[x] == '-') expsign = s[x];
                    stg = Stage.exponent;
                }
                else if ('0' <= s[x] && s[x] <= '9')
                {
                    if (stg == Stage.integer) integer += s[x];
                    else if (stg == Stage.fraction) fraction += s[x];
                    else if (stg == Stage.exponent) exponent += s[x];
                }
                else
                {
                    if (String.IsNullOrEmpty(integer)) throw new Exception();
                    if (stg == Stage.exponent && String.IsNullOrEmpty(exponent)) throw new Exception();
                    break;
                }
            }

            BigRational d = UnicodeCore.InterpretAsInteger(new UnicodeStream(integer)) + UnicodeCore.InterpretAsMantissa(new UnicodeStream(fraction));

            if (!String.IsNullOrEmpty(exponent))
            {
                d = BigRational.Pow(d, Convert.ToInt32(exponent));
                if (expsign == '-') d = 1 / d;
            }

            if (negative) d *= -1;
            return d;
        }

        public static ManagedBoolean ReadBooleanTrueAt(ref int i, string s)
        {
            if (s.Substring(i, 4).Equals("true"))
            {
                i += 4;
                return true;
            }
            else throw new Exception();
        }

        public static ManagedBoolean ReadBooleanFalseAt(ref int i, string s)
        {
            if (s.Substring(i, 5).Equals("false"))
            {
                i += 5;
                return false;
            }
            else throw new Exception();
        }

        public static ManagedNull ReadNullAt(ref int i, string s)
        {
            if (s.Substring(i, 4).Equals("null"))
            {
                i += 4;
                return new ManagedNull();
            }
            else throw new Exception();
        }

        private static void EatStuffing(ref int i, string s)
        {
            while (true)
            {
                if (i != s.Length - 1)
                    if (s[i] == ' ' ^ s[i] == '\t' ^ s[i] == '\r' ^ s[i] == '\n') i++;
                    else return;
                else return;
            }
        }

        public static ValueType GuessElementTypeAt(int i, string s)
        {
            EatStuffing(ref i, s);
            if (i == s.Length) return ValueType.None;
            else return GuessValueTypeAt(i, s);
        }

        public static object ReadElementAt(ref int i, string s)
        {
            EatStuffing(ref i, s);
            if (i == s.Length) throw new Exception();
            object value = ReadValueAt(ref i, s);
            EatStuffing(ref i, s);
            return value;
        }

        public static object ReadValueAt(ref int i, string s)
        {
            switch (GuessValueTypeAt(i, s))
            {
                case ValueType.Object: return ReadObjectAt(ref i, s);
                case ValueType.Array: return ReadArrayAt(ref i, s);
                case ValueType.String: return ReadStringAt(ref i, s);
                case ValueType.Number: return ReadNumberAt(ref i, s);
                case ValueType.BooleanTrue: return ReadBooleanTrueAt(ref i, s);
                case ValueType.BooleanFalse: return ReadBooleanFalseAt(ref i, s);
                case ValueType.Null: return ReadNullAt(ref i, s);
                default: return null;
            }
        }
    }

    // UTFStreams
    /* public partial class JSONCore
    {
        static ValueType GuessValueTypeAt(int i, UTFStream s)
        {
            switch (s[i])
            {
                case UTFConsts.CurlyBracketOpen:
                    return ValueType.Object;
                case UTFConsts.SquareBracketOpen:
                    return ValueType.Array;
                case UTFConsts.Quote:
                    return ValueType.String;
                case UTFConsts.t:
                    return ValueType.BooleanTrue;
                case UTFConsts.f:
                    return ValueType.BooleanFalse;
                case UTFConsts.n:
                    return ValueType.Null;
                default:
                    return ValueType.Number;
            }
        }

        static UTFContainer ReadObjectAt(ref int i, UTFStream s)
        {
            if (s.Length - i < 2)
                throw new Exception();

            if (s[i] != UTFConsts.CurlyBracketOpen)
                throw new Exception();

            UTFContainer obj = new UTFContainer();
            i++;
            EatStuffing(ref i, s);

            if (s[i] != UTFConsts.CurlyBracketClose)
            {
                do
                {
                    obj.Add((KeyValuePair<UTFStream, object>)ReadMemberAt(ref i, s));

                    if (s[i] != UTFConsts.Comma)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
                while (true);

                if (s[i] != UTFConsts.CurlyBracketClose)
                    throw new Exception();
            }

            return obj;
        }

        static ArrayContainer ReadArrayAt(ref int i, UTFStream s)
        {
            if (s.Length - i < 2)
                throw new Exception();

            if (s[i] != UTFConsts.SquareBracketOpen)
                throw new Exception();

            ArrayContainer obj = new ArrayContainer();
            int index = 0;
            i++;
            EatStuffing(ref i, s);

            if (s[i] != UTFConsts.SquareBracketClose)
            {
                do
                {
                    obj.Add(new KeyValuePair<int, object>(index, ReadElementAt(ref i, s)));

                    if (s[i] != UTFConsts.Comma)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                        index++;
                    }
                }
                while (true);

                if (s[i] != UTFConsts.SquareBracketClose)
                    throw new Exception();
            }

            i++;
            return obj;
        }

        static KeyValuePair<UTFStream, object> ReadMemberAt(ref int i, UTFStream s)
        {
            EatStuffing(ref i, s);
            UTFStream key = ReadStringAt(ref i, s);
            EatStuffing(ref i, s);

            if (s[i] == UTFConsts.Colon)
            {
                i++;
                object val = ReadElementAt(ref i, s);
                return new KeyValuePair<UTFStream, object>(key, val);
            }
            else
            {
                throw new Exception();
            }
        }

        static UTFStream ReadStringAt(ref int i, UTFStream s)
        {
            if (s[i] != UTFConsts.Quote)
            {
                throw new Exception();
            }

            UTFStream str = new UTFStream();

            for (int x = ++i; x < s.Length; x++, i++)
            {
                if (0x0020 <= s[x] && s[x] <= 0x10ffff)
                {
                    switch (s[x])
                    {
                        case UTFConsts.Quote:
                            i++;
                            return str;
                        case UTFConsts.BackSlash:
                            if (x != s.Length - 1)
                            {
                                x++;
                                switch (s[x])
                                {
                                    case UTFConsts.b:
                                        str.Push(UTFConsts.BackSpace);
                                        break;
                                    case UTFConsts.f:
                                        str.Push(UTFConsts.FormFeed);
                                        break;
                                    case UTFConsts.t:
                                        str.Push(UTFConsts.Tab);
                                        break;
                                    case UTFConsts.BackSlash:
                                        str.Push(UTFConsts.BackSlash);
                                        break;
                                    case UTFConsts.ForwardSlash:
                                        str.Push(UTFConsts.ForwardSlash);
                                        break;
                                    case UTFConsts.u:
                                        x++;
                                        if (!(x + 3 <= s.Length - 1))
                                            throw new Exception();

                                        UTFStream hex = new UTFStream();

                                        for (int _x = x; _x < x + 4; _x++)
                                        {
                                            if (UTFUtil.IsDigit(s[_x]) ^ UTFUtil.IsLowerCaseLetter(s[_x]) ^ UTFUtil.IsUpperCaseLetter(s[_x]))
                                            {
                                                hex.Push(s[_x]);
                                            }
                                            else
                                            {
                                                throw new Exception();
                                            }
                                        }

                                        str.Push(UTFUtil.Hex2Decimal(hex));
                                        break;
                                    default:
                                        throw new Exception();
                                }
                            }

                            break;
                        default:
                            str.Push(s[x]);
                            break;
                    }
                }
            }

            throw new Exception();
        }

        static ManagedBigRational ReadNumberAt(ref int i, UTFStream s)
        {
            bool negative = false;

            if (s[i] == UTFConsts.Minus)
            {
                negative = true;
                i++;
            }

            Stage stg = Stage.integer;
            UTFStream integer = new UTFStream();
            UTFStream fraction = new UTFStream();
            UInt64 expsign = UTFConsts.Plus;
            UTFStream exponent = new UTFStream();

            for (int x = i; x < s.Length; x++, i++)
            {
                if (s[x] == UTFConsts.Dot)
                {
                    if (integer.IsEmpty)
                        throw new Exception();

                    if (stg != Stage.integer)
                        throw new Exception();

                    stg = Stage.fraction;
                }
                else if (s[x] == UTFConsts.e || s[x] == UTFConsts.E)
                {
                    if (integer.IsEmpty)
                        throw new Exception();

                    if (stg == Stage.exponent)
                        throw new Exception();

                    if (x == s.Length - 1)
                        throw new Exception();

                    if (stg == Stage.fraction && fraction.IsEmpty)
                        throw new Exception();

                    x++;
                    i++;

                    if (s[x] == UTFConsts.Plus ^ s[x] == UTFConsts.Minus)
                    {
                        expsign = s[x];
                    }

                    stg = Stage.exponent;
                }
                else if (UTFUtil.IsDigit(s[x]))
                {
                    if (stg == Stage.integer)
                    {
                        integer.Push(s[x]);
                    }
                    else if (stg == Stage.fraction)
                    {
                        fraction.Push(s[x]);
                    }
                    else if (stg == Stage.exponent)
                    {
                        exponent.Push(s[x]);
                    }
                }
                else
                {
                    if (integer.IsEmpty)
                        throw new Exception();

                    if (stg == Stage.exponent && exponent.IsEmpty)
                        throw new Exception();

                    break;
                }
            }

            BigRational d = UTFUtil.InterpretAsInteger(integer) + UTFUtil.InterpretAsMantissa(fraction);

            if (!exponent.IsEmpty)
            {
                d = BigRational.Pow(d, UTFUtil.InterpretAsInteger(exponent));

                if (expsign == UTFConsts.Minus)
                {
                    d = 1 / d;
                }
            }

            if (negative)
                d *= -1;

            return d;
        }

        static bool ReadBooleanTrueAt(ref int i, UTFStream s)
        {
            if (Equals(s[i, 4], new UInt64[4] { UTFConsts.t, UTFConsts.r, UTFConsts.u, UTFConsts.e }))
            {
                i += 4;
                return true;
            }
            else
            {
                throw new Exception();
            }
        }

        static bool ReadBooleanFalseAt(ref int i, UTFStream s)
        {
            if (((UTFStream)s[i, 5]).Same(new UInt64[] { UTFConsts.f, UTFConsts.a, UTFConsts.l, UTFConsts.s, UTFConsts.e }))
            {
                i += 5;
                return false;
            }
            else
            {
                throw new Exception();
            }
        }

        static object ReadNullAt(ref int i, UTFStream s)
        {
            if (((UTFStream)s[i, 4]).Same(new UInt64[] { UTFConsts.n, UTFConsts.u, UTFConsts.l, UTFConsts.l }))
            {
                i += 4;
                return null;
            }
            else
            {
                throw new Exception();
            }
        }

        public static void EatStuffing(ref int i, UTFStream s)
        {
            while (true)
            {
                if (i != s.Length - 1)
                    if (s[i] == UTFConsts.Space ^ s[i] == UTFConsts.Tab ^ s[i] == UTFConsts.CarriageReturn ^ s[i] == UTFConsts.LineFeed)
                        i++;
                    else
                        return;
                else
                    return;
            }
        }

        public static object ReadElementAt(ref int i, UTFStream s)
        {
            EatStuffing(ref i, s);

            if (i == s.Length)
                throw new Exception();

            object value = ReadValueAt(ref i, s);
            EatStuffing(ref i, s);
            return value;
        }

        public static object ReadValueAt(ref int i, UTFStream s)
        {
            switch (GuessValueTypeAt(i, s))
            {
                case ValueType.Object:
                    return ReadObjectAt(ref i, s);
                case ValueType.Array:
                    return ReadArrayAt(ref i, s);
                case ValueType.String:
                    return ReadStringAt(ref i, s);
                case ValueType.Number:
                    return ReadNumberAt(ref i, s);
                case ValueType.BooleanTrue:
                    return ReadBooleanTrueAt(ref i, s);
                case ValueType.BooleanFalse:
                    return ReadBooleanFalseAt(ref i, s);
                case ValueType.Null:
                    return ReadNullAt(ref i, s);
                default:
                    return null;
            }
        }
    } */
}
