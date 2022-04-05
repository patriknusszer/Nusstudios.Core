namespace Nusstudios.Core.ManagedTypes
{
    public abstract class ManagedUnsignedInteger : ManagedInteger
    {
        public abstract void Set(ManagedUnsignedInteger value);

        public static explicit operator float(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return (float)mui32;
            else return (float)(ManagedUInt64)op;
        }

        public static explicit operator double(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return mui32;
            else return (double)(ManagedUInt64)op;
        }

        public static explicit operator decimal(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return mui32;
            else return (ManagedUInt64)op;
        }

        public static explicit operator sbyte(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return (sbyte)mui8;
            else if (op is ManagedUInt16 mui16) return (sbyte)mui16;
            else if (op is ManagedUInt32 mui32) return (sbyte)mui32;
            else return (sbyte)(ManagedUInt64)op;
        }

        public static explicit operator short(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return (short)mui16;
            else if (op is ManagedUInt32 mui32) return (short)mui32;
            else return (short)(ManagedUInt64)op;
        }

        public static explicit operator int(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return (int)mui32;
            else return (int)(ManagedUInt64)op;
        }

        public static explicit operator long(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return mui32;
            else return (long)(ManagedUInt64)op;
        }

        public static explicit operator byte(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return (byte)mui16;
            else if (op is ManagedUInt32 mui32) return (byte)mui32;
            else return (byte)(ManagedUInt64)op;
        }

        public static explicit operator ushort(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return (ushort)mui32;
            else return (ushort)(ManagedUInt64)op;
        }

        public static explicit operator uint(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return mui32;
            else return (uint)(ManagedUInt64)op;
        }

        public static explicit operator ulong(ManagedUnsignedInteger op)
        {
            if (op is ManagedUInt8 mui8) return mui8;
            else if (op is ManagedUInt16 mui16) return mui16;
            else if (op is ManagedUInt32 mui32) return mui32;
            else return (ManagedUInt64)op;
        }

        public static implicit operator ManagedUnsignedInteger(byte op) => new ManagedUInt8(op);
        public static implicit operator ManagedUnsignedInteger(ushort op) => new ManagedUInt16(op);
        public static implicit operator ManagedUnsignedInteger(uint op) => new ManagedUInt32(op);
        public static implicit operator ManagedUnsignedInteger(ulong op) => new ManagedUInt64(op);
    }
}
