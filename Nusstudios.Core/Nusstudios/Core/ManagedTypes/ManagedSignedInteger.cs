using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public abstract class ManagedSignedInteger : ManagedInteger
    {
        public abstract void Set(ManagedSignedInteger value);

        public static explicit operator float(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return (float)mi32;
            else return (float)(ManagedInt64)op;
        }

        public static explicit operator double(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return mi32;
            else return (double)(ManagedInt64)op;
        }

        public static explicit operator decimal(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return mi32;
            else return (ManagedInt64)op;
        }

        public static explicit operator BigRational(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return mi32;
            else return (ManagedInt64)op;
        }

        public static explicit operator sbyte(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return (sbyte)mi16;
            else if (op is ManagedInt32 mi32) return (sbyte)mi32;
            else return (sbyte)(ManagedInt64)op;
        }

        public static explicit operator short(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return (short)mi32;
            else return (short)(ManagedInt64)op;
        }

        public static explicit operator int(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return mi32;
            else return (int)(ManagedInt64)op;
        }

        public static explicit operator long(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return mi8;
            else if (op is ManagedInt16 mi16) return mi16;
            else if (op is ManagedInt32 mi32) return mi32;
            else return (ManagedInt64)op;
        }

        public static explicit operator byte(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return (byte)mi8;
            else if (op is ManagedInt16 mi16) return (byte)mi16;
            else if (op is ManagedInt32 mi32) return (byte)mi32;
            else return (byte)(ManagedInt64)op;
        }

        public static explicit operator ushort(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return (ushort)mi8;
            else if (op is ManagedInt16 mi16) return (ushort)mi16;
            else if (op is ManagedInt32 mi32) return (ushort)mi32;
            else return (ushort)(ManagedInt64)op;
        }

        public static explicit operator uint(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return (uint)mi8;
            else if (op is ManagedInt16 mi16) return (uint)mi16;
            else if (op is ManagedInt32 mi32) return (uint)mi32;
            else return (uint)(ManagedInt64)op;
        }

        public static explicit operator ulong(ManagedSignedInteger op)
        {
            if (op is ManagedInt8 mi8) return (ulong)mi8;
            else if (op is ManagedInt16 mi16) return (ulong)mi16;
            else if (op is ManagedInt32 mi32) return (ulong)mi32;
            else return (ulong)(ManagedInt64)op;
        }

        public static implicit operator ManagedSignedInteger(sbyte op) => new ManagedInt8(op);
        public static implicit operator ManagedSignedInteger(short op) => new ManagedInt16(op);
        public static implicit operator ManagedSignedInteger(int op) => new ManagedInt32(op);
        public static implicit operator ManagedSignedInteger(long op) => new ManagedInt64(op);
    }
}
