using System;
using System.Linq;
using System.Collections.Generic;
using Nusstudios.Core.ManagedTypes;

namespace Nusstudios.Core.Parsing.Unicode

{
    public class UnicodeStream
    {
        public bool Same(UnicodeStream stream)
        {
            return CodePoints.SequenceEqual(stream.CodePoints);
        }

        public int IndexOf(string str)
        {
            return IndexOf((UnicodeStream)str);
        }

        public int IndexOf(char c)
        {
            return IndexOf((UnicodeStream)c);
        }

        public int IndexOf(UnicodeStream stream)
        {
            return IndexOf(stream.CodePoints.ToArray());
        }

        public int IndexOf(UInt64[] codepoints)
        {
            return ArrayUtil.IndexOfSubArray<UInt64>(CodePoints.ToArray(), codepoints);
        }

        public int LastIndexOf(string str)
        {
            return LastIndexOf((UnicodeStream)str);
        }

        public int LastIndexOf(char c)
        {
            return LastIndexOf((UnicodeStream)c);
        }

        public int LastIndexOf(UnicodeStream stream)
        {
            return LastIndexOf(stream.CodePoints.ToArray());
        }

        public int LastIndexOf(UInt64[] codepoints)
        {
            return ArrayUtil.LastIndexOfSubArray<UInt64>(CodePoints.ToArray(), codepoints);
        }

        public UInt64 this[int i]
        {
            get
            {
                return this.CodePoints[i];
            }
            set
            {
                this.CodePoints[i] = value;
            }
        }

        public UInt64[] this[int i, int x]
        {
            get
            {
                return this.CodePoints.GetRange(i, x).ToArray();
            }
        }

        public List<UInt64> CodePoints = new List<ulong>();

        public int Length
        {
            get
            {
                return CodePoints.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return CodePoints.Count == 0;
            }
        }

        public UnicodeStream[] Split(UnicodeStream delimiter, bool removeEmptyEntries)
        {
            UInt64[][] arr = ArrayUtil.SplitArray(CodePoints.ToArray(), delimiter.CodePoints.ToArray(), removeEmptyEntries);
            UnicodeStream[] result = new UnicodeStream[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                result[i] = new UnicodeStream(arr[i]);

            return result;
        }

        public UInt64[][] Split(UInt64[] delimiter, bool removeEmptyEntries)
        {
            return ArrayUtil.KMPSplit(CodePoints.ToArray(), delimiter, removeEmptyEntries);
        }

        public UInt64[][] Split(UInt64 delimiter, bool removeEmptyEntries)
        {
            return ArrayUtil.SplitArray(CodePoints.ToArray(), delimiter, removeEmptyEntries);
        }

        public void Push(UInt64 codepoint)
        {
            CodePoints.Add(codepoint);
        }

        public void Push(char c)
        {
            PushRange(new char[] { c });
        }

        public void PushRange(char[] c)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(c);
            CodePoints.AddRange(UnicodeCore.parseUTFStreamToCodePoints(encoded, Encoding.UTF8));
        }

        public void Push(UnicodeStream stream)
        {
            CodePoints.AddRange(stream.CodePoints);
        }

        public void Push(string str)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(str);
            CodePoints.AddRange(UnicodeCore.parseUTFStreamToCodePoints(encoded, Encoding.UTF8));
        }

        public void Insert(int index, UInt64 codepoint)
        {
            CodePoints.Insert(index, codepoint);
        }

        public void Insert(int index, string str)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(str);
            CodePoints.InsertRange(index, UnicodeCore.parseUTFStreamToCodePoints(encoded, Encoding.UTF8));
        }

        public void Insert(int index, UnicodeStream str)
        {
            CodePoints.InsertRange(index, str.CodePoints);
        }

        public void PushRange(UInt64[] codepoint)
        {
            CodePoints.AddRange(codepoint);
        }

        public void PushRange(string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
                Push(strs[i]);
        }

        public void InsertRange(int index, UInt64[] codepoint)
        {
            CodePoints.InsertRange(index, codepoint);
        }

        public void InsertRange(int index, string[] strs)
        {
            List<UInt64> cps = new List<ulong>();

            for (int i = 0; i < strs.Length; i++)
                cps.AddRange(UnicodeCore.parseUTFStreamToCodePoints(System.Text.Encoding.UTF8.GetBytes(strs[i]), Encoding.UTF8));

            CodePoints.InsertRange(index, cps);
        }

        public void Delete(int index)
        {
            CodePoints.RemoveAt(index);
        }

        public void DeleteRange(int index, int count)
        {
            CodePoints.RemoveRange(index, count);
        }

        public ulong CharAt(int index)
        {
            return CodePoints[index];
        }

        public UnicodeStream GetRange(int index, int count)
        {
            return CodePoints.GetRange(index, count).ToArray();
        }

        public UnicodeStream GetRange(int index)
        {
            return GetRange(index, Length - index);
        }

        public byte[] EncodeToStream(Encoding enc)
        {
            return UnicodeCore.parseCodePointsToUTFStream(CodePoints, enc);
        }

        public UnicodeStream(byte[] buffer, Encoding enc)
        {
            CodePoints = UnicodeCore.parseUTFStreamToCodePoints(buffer, enc);
        }

        public UnicodeStream(UInt64[] codepoints)
        {
            CodePoints.AddRange(codepoints);
        }

        public UnicodeStream(UInt64 codepoint)
        {
            CodePoints.Add(codepoint);
        }

        public UnicodeStream(List<UInt64> codepoints)
        {
            CodePoints.AddRange(codepoints);
        }

        public UnicodeStream(string str)
        {
            CodePoints = UnicodeCore.parseUTFStreamToCodePoints(System.Text.Encoding.UTF8.GetBytes(str), Encoding.UTF8);
        }

        public UnicodeStream(char c)
        {
            CodePoints = UnicodeCore.parseUTFStreamToCodePoints(System.Text.Encoding.UTF8.GetBytes(new char[] { c }), Encoding.UTF8);
        }

        public UnicodeStream() { }

        public static UnicodeStream operator +(UnicodeStream lhs, UnicodeStream rhs)
        {
            lhs.CodePoints.AddRange(rhs.CodePoints);
            return new UnicodeStream(lhs.CodePoints);
        }

        public override string ToString()
        {
            return System.Text.Encoding.UTF8.GetString(UnicodeCore.parseCodePointsToUTFStream(CodePoints, Encoding.UTF8));
        }

        public string ToString(int index)
        {
            return System.Text.Encoding.UTF8.GetString(UnicodeCore.parseCodePointsToUTFStream(CodePoints.GetRange(index, CodePoints.Count - index), Encoding.UTF8));
        }

        public string ToString(int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString(UnicodeCore.parseCodePointsToUTFStream(CodePoints.GetRange(index, count), Encoding.UTF8));
        }

        public static implicit operator UnicodeStream(string op) => new UnicodeStream(op);
        public static implicit operator UnicodeStream(char op) => new UnicodeStream(op);
        public static implicit operator UnicodeStream(ulong[] op) => new UnicodeStream(op);
        public static implicit operator UnicodeStream(ulong op) => new UnicodeStream(op);
        public static implicit operator ulong[](UnicodeStream op) => op.CodePoints.ToArray();
        public static implicit operator string(UnicodeStream op) => op.ToString();
    }
}
