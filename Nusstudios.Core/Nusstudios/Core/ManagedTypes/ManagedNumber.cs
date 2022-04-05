using Nusstudios.Core.UnmanagedTypes;
using Nusstudios.Core.Parsing.JSON;

namespace Nusstudios.Core.ManagedTypes
{
    public abstract class ManagedNumber : JLeaf
    {
        public abstract void Set(ManagedNumber value);

        public static explicit operator float(ManagedNumber op) => op is ManagedInteger mi ? (float)mi : (float)(ManagedRational)op;
        public static explicit operator double(ManagedNumber op) => op is ManagedInteger mi ? (double)mi : (double)(ManagedRational)op;
        public static explicit operator decimal(ManagedNumber op) => op is ManagedInteger mi ? (decimal)mi : (decimal)(ManagedRational)op;
        public static explicit operator BigRational(ManagedNumber op) => op is ManagedInteger mi ? (BigRational)mi : (BigRational)(ManagedRational)op;
        public static explicit operator byte(ManagedNumber op) => op is ManagedInteger mi ? (byte)mi : (byte)(ManagedRational)op;
        public static explicit operator ushort(ManagedNumber op) => op is ManagedInteger mi ? (ushort)mi : (ushort)(ManagedRational)op;
        public static explicit operator uint(ManagedNumber op) => op is ManagedInteger mi ? (uint)mi : (uint)(ManagedRational)op;
        public static explicit operator ulong(ManagedNumber op) => op is ManagedInteger mi ? (ulong)mi : (ulong)(ManagedRational)op;
        public static explicit operator sbyte(ManagedNumber op) => op is ManagedInteger mi ? (sbyte)mi : (sbyte)(ManagedRational)op;
        public static explicit operator short(ManagedNumber op) => op is ManagedInteger mi ? (short)mi : (short)(ManagedRational)op;
        public static explicit operator int(ManagedNumber op) => op is ManagedInteger mi ? (int)mi : (int)(ManagedRational)op;
        public static explicit operator long(ManagedNumber op) => op is ManagedInteger mi ? (long)mi : (long)(ManagedRational)op;

        public static implicit operator ManagedNumber(float op) => new ManagedFloat(op);
        public static implicit operator ManagedNumber(double op) => new ManagedDouble(op);
        public static implicit operator ManagedNumber(decimal op) => new ManagedDecimal(op);
        public static implicit operator ManagedNumber(BigRational op) => new BigRational(op);
        public static implicit operator ManagedNumber(byte op) => new ManagedUInt8(op);
        public static implicit operator ManagedNumber(ushort op) => new ManagedUInt16(op);
        public static implicit operator ManagedNumber(uint op) => new ManagedUInt32(op);
        public static implicit operator ManagedNumber(ulong op) => new ManagedUInt64(op);
        public static implicit operator ManagedNumber(sbyte op) => new ManagedInt8(op);
        public static implicit operator ManagedNumber(short op) => new ManagedInt16(op);
        public static implicit operator ManagedNumber(int op) => new ManagedInt32(op);
        public static implicit operator ManagedNumber(long op) => new ManagedInt64(op);
    }
}
