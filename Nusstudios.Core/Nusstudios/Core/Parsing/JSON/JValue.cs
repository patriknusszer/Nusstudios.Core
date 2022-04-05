using System;
using System.Collections;
using Nusstudios.Core.Mapping;
using System.Collections.Generic;
using Nusstudios.Core.ManagedTypes;
using Nusstudios.Core.UnmanagedTypes;
using Nusstudios.Core.Mapping.Collections;

namespace Nusstudios.Core.Parsing.JSON
{
    public abstract class JValue: IEnumerable<KeyValuePair<object, JValue>>
    {
        public IEnumerator<KeyValuePair<object, JValue>> GetEnumerator()
        {
            if (this is JContainer jc) return jc.GetEnumerator();
            else throw new Exception("Not a JContainer");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public JValue Copy() => this.DeepClone();
        public abstract JValue this[object key] { get; set; }
        public static implicit operator JValue(sbyte op) => (ManagedNumber)op;
        public static implicit operator JValue(short op) => (ManagedNumber)op;
        public static implicit operator JValue(int op) => (ManagedNumber)op;
        public static implicit operator JValue(long op) => (ManagedNumber)op;
        public static implicit operator JValue(byte op) => (ManagedNumber)op;
        public static implicit operator JValue(ushort op) => (ManagedNumber)op;
        public static implicit operator JValue(uint op) => (ManagedNumber)op;
        public static implicit operator JValue(ulong op) => (ManagedNumber)op;
        public static implicit operator JValue(float op) => (ManagedNumber)op;
        public static implicit operator JValue(double op) => (ManagedNumber)op;
        public static implicit operator JValue(decimal op) => (ManagedNumber)op;
        public static implicit operator JValue(BigRational op) => (ManagedNumber)op;
        public static implicit operator JValue(bool op) => (ManagedBoolean)op;
        public static implicit operator JValue(string op) => (ManagedString)op;

        public static explicit operator sbyte(JValue op) => (sbyte)(ManagedNumber)op;
        public static explicit operator short(JValue op) => (short)(ManagedNumber)op;
        public static explicit operator int(JValue op) => (int)(ManagedNumber)op;
        public static explicit operator long(JValue op) => (long)(ManagedNumber)op;
        public static explicit operator byte(JValue op) => (byte)(ManagedNumber)op;
        public static explicit operator ushort(JValue op) => (ushort)(ManagedNumber)op;
        public static explicit operator uint(JValue op) => (uint)(ManagedNumber)op;
        public static explicit operator ulong(JValue op) => (ulong)(ManagedNumber)op;
        public static explicit operator float(JValue op) => (float)(ManagedNumber)op;
        public static explicit operator double(JValue op) => (double)(ManagedNumber)op;
        public static explicit operator decimal(JValue op) => (decimal)(ManagedNumber)op;
        public static explicit operator BigRational(JValue op) => (BigRational)(ManagedNumber)op;
        public static explicit operator bool(JValue op) => (ManagedBoolean)op;
        public static explicit operator string(JValue op) => (ManagedString)op;

        public string ToJSONString(JIndentCfg cfg)
        {
            if (this is JContainer jc) return JSONCore.WriteValue(jc.content, 0, cfg);
            else return JSONCore.WriteValue(this, 0, cfg);
        }

        public string ToJSONString()
        {
            if (this is JContainer jc) return JSONCore.WriteValue(jc.content);
            else return JSONCore.WriteValue(this);
        }

        public static JValue Parse(string s)
        {
            ValueType t;
            if ((t = JSONCore.GuessElementTypeAt(0, s)).Equals(ValueType.None)) throw new Exception("Provided string is not a valid JSON value");
            int i = 0;
            object content = JSONCore.ReadElementAt(ref i, s);
            if (t == ValueType.Object) return JObject.CreateFromRef((StringContainer)content);
            else if (t == ValueType.Array) return JArray.CreateFromRef((ArrayContainer)content);
            else return (JValue)content;
        }
    }
}
