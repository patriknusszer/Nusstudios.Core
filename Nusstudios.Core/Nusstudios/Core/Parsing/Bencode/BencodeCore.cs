using System;
using System.Text;
using System.Collections.Generic;

namespace Nusstudios.Core.Core.Parsing.Bencode
{
    public static class BencodeCore
    {
        public enum Type
        {
            Dictionary,
            List,
            ByteString,
            Number
        }

        public static Type GuessTypeAt(ref int i, byte[] s)
        {
            switch (AsASCII(s[i]))
            {
                case 'i': return Type.Number;
                case 'd': return Type.Dictionary;
                case 'l': return Type.List;
                default: return Char.IsDigit(AsASCII(s[i])) ? Type.ByteString : throw new Exception("Can not determine type");
            }
        }

        public static object ReadValueAt(ref int i, byte[] s)
        {
            switch (GuessTypeAt(ref i, s))
            {
                case Type.Dictionary: return ReadDictionaryAt(ref i, s);
                case Type.List: return ReadListAt(ref i, s);
                case Type.ByteString: return ReadByteStringAt(ref i, s);
                default: return ReadNumberAt(ref i, s);
            }
        }

        public static SortedDictionary<string, object> ReadDictionaryAt(ref int i, byte[] s)
        {
            if (!AsASCII(s[i]).Equals('d')) throw new Exception("Not a dictionary");
            i++;
            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();

            while (!AsASCII(s[i]).Equals('e'))
            {
                string key = ReadASCIIStringAt(ref i, s);
                object value = ReadValueAt(ref i, s);
                dict[key] = value;
            }

            i++;
            return dict;
        }

        public static List<object> ReadListAt(ref int i, byte[] s)
        {
            if (!AsASCII(s[i]).Equals('l')) throw new Exception("Not a list");
            i++;
            List<object> lst = new List<object>();
            while (!AsASCII(s[i]).Equals('e')) lst.Add(ReadValueAt(ref i, s));
            i++;
            return lst;
        }

        public static byte FromASCII(char s) => Encoding.ASCII.GetBytes(s.ToString())[0];
        public static byte[] FromASCII(string s) => Encoding.ASCII.GetBytes(s);
        public static char AsASCII(byte s) => Encoding.ASCII.GetString(new byte[] { s })[0];
        public static string ReadASCIIStringAt(ref int i, byte[] s) => Encoding.ASCII.GetString(ReadByteStringAt(ref i, s));

        public static byte[] ReadByteStringAt(ref int i, byte[] s)
        {
            string _length = "";
            while (Char.IsDigit(AsASCII(s[i]))) _length += AsASCII(s[i++]);
            if (!AsASCII(s[i]).Equals(':') || _length.Length == 0) throw new Exception("Not a bytestring");
            i++;
            int length = Convert.ToInt32(_length);
            byte[] bytestring = new byte[length];
            if (s.Length - i < length) throw new Exception("Not a bytestring" + s[i]);
            Array.Copy(s, i, bytestring, 0, bytestring.Length);
            i += length;
            return bytestring;
        }

        public static long ReadNumberAt(ref int i, byte[] s)
        {
            if (!AsASCII(s[i]).Equals('i')) throw new Exception("Not a number");
            i++;
            string _number = "";
            while (Char.IsDigit(AsASCII(s[i]))) _number += AsASCII(s[i++]);
            if (!AsASCII(s[i]).Equals('e')) throw new Exception("Not a number");
            i++;
            return Convert.ToInt64(_number);
        }

        public static byte[] WriteDictionary(SortedDictionary<string, object> s)
        {
            List<byte> tmp = new List<byte>();
            tmp.Add(FromASCII('d'));

            foreach (KeyValuePair<string, object> kvp in s)
            {
                string key = kvp.Key;
                tmp.AddRange(WriteASCIIString(key));
                object value = kvp.Value;

                if (value is byte[] barr) tmp.AddRange(WriteByteString(barr));
                else if (value is Int64 i64) tmp.AddRange(WriteNumber(i64));
                else if (value is String str) tmp.AddRange(WriteASCIIString(str));
                else if (value is SortedDictionary<string, object> sd) tmp.AddRange(WriteDictionary(sd));
                else if (value is List<object> l) tmp.AddRange(WriteList(l));
                else throw new Exception("Type not supported");
            }

            tmp.Add(FromASCII('e'));
            return tmp.ToArray();
        }

        public static byte[] WriteList(List<object> s)
        {
            List<byte> tmp = new List<byte>();
            tmp.Add(FromASCII('l'));

            for (int i = -1; ++i < s.Count;)
            {
                object value = s[i];

                if (value is byte[] barr) tmp.AddRange(WriteByteString(barr));
                else if (value is Int64 i64) tmp.AddRange(WriteNumber(i64));
                else if (value is String str) tmp.AddRange(WriteASCIIString(str));
                else if (value is SortedDictionary<string, object> sd) tmp.AddRange(WriteDictionary(sd));
                else if (value is List<object> l) tmp.AddRange(WriteList(l));
                else throw new Exception("Type not supported");
            }

            tmp.Add(FromASCII('e'));
            return tmp.ToArray();
        }

        public static byte[] WriteASCIIString(string s) => WriteByteString(Encoding.ASCII.GetBytes(s));

        public static byte[] WriteByteString(byte[] s)
        {
            List<byte> tmp = new List<byte>();
            tmp.AddRange(FromASCII(s.Length.ToString()));
            tmp.Add(FromASCII(':'));
            tmp.AddRange(s);
            return tmp.ToArray();
        }

        public static byte[] WriteNumber(Int64 s)
        {
            List<byte> tmp = new List<byte>();
            tmp.Add(FromASCII('i'));
            tmp.AddRange(FromASCII(s.ToString()));
            tmp.Add(FromASCII('e'));
            return tmp.ToArray();
        }
    }
}
