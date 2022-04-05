using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public abstract class ManagedInteger : ManagedNumber {
        public abstract void Set(ManagedInteger value);

        public static explicit operator float(ManagedInteger op) => op is ManagedSignedInteger msi ? (float)msi : (float)(ManagedUnsignedInteger)op;
        public static explicit operator double(ManagedInteger op) => op is ManagedSignedInteger msi ? (double)msi : (double)(ManagedUnsignedInteger)op;
        public static explicit operator decimal(ManagedInteger op) => op is ManagedSignedInteger msi ? (decimal)msi : (decimal)(ManagedUnsignedInteger)op;
        public static explicit operator BigRational(ManagedInteger op) => op is ManagedSignedInteger msi ? (BigRational)msi : (BigRational)(ManagedUnsignedInteger)op;
        public static explicit operator byte(ManagedInteger op) => op is ManagedSignedInteger msi ? (byte)msi : (byte)(ManagedUnsignedInteger)op;
        public static explicit operator ushort(ManagedInteger op) => op is ManagedSignedInteger msi ? (ushort)msi : (ushort)(ManagedUnsignedInteger)op;
        public static explicit operator uint(ManagedInteger op) => op is ManagedSignedInteger msi ? (uint)msi : (uint)(ManagedUnsignedInteger)op;
        public static explicit operator ulong(ManagedInteger op) => op is ManagedSignedInteger msi ? (ulong)msi : (ulong)(ManagedUnsignedInteger)op;
        public static explicit operator sbyte(ManagedInteger op) => op is ManagedSignedInteger msi ? (sbyte)msi : (sbyte)(ManagedUnsignedInteger)op;
        public static explicit operator short(ManagedInteger op) => op is ManagedSignedInteger msi ? (short)msi : (short)(ManagedUnsignedInteger)op;
        public static explicit operator int(ManagedInteger op) => op is ManagedSignedInteger msi ? (int)msi : (int)(ManagedUnsignedInteger)op;
        public static explicit operator long(ManagedInteger op) => op is ManagedSignedInteger msi ? (long)msi : (long)(ManagedUnsignedInteger)op;

        public static implicit operator ManagedInteger(byte op) => new ManagedUInt8(op);
        public static implicit operator ManagedInteger(ushort op) => new ManagedUInt16(op);
        public static implicit operator ManagedInteger(uint op) => new ManagedUInt32(op);
        public static implicit operator ManagedInteger(ulong op) => new ManagedUInt64(op);
        public static implicit operator ManagedInteger(sbyte op) => new ManagedInt8(op);
        public static implicit operator ManagedInteger(short op) => new ManagedInt16(op);
        public static implicit operator ManagedInteger(int op) => new ManagedInt32(op);
        public static implicit operator ManagedInteger(long op) => new ManagedInt64(op);
    }
}
