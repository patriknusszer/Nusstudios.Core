using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public abstract class ManagedRational : ManagedNumber
    {
        public abstract void Set(ManagedRational value);

        public static explicit operator float(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (float)mbr;
            else if (d is ManagedDecimal mde) return (float)mde;
            else if (d is ManagedDouble mdo) return (float)mdo;
            else return (ManagedFloat)d;
        }

        public static explicit operator double(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (double)mbr;
            else if (d is ManagedDecimal mde) return (double)mde;
            else if (d is ManagedDouble mdo) return mdo;
            else return (ManagedFloat)d;
        }

        public static explicit operator decimal(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (decimal)mbr;
            else if (d is ManagedDecimal mde) return mde;
            else if (d is ManagedDouble mdo) return (decimal)mdo;
            else return (decimal)(ManagedFloat)d;
        }

        public static explicit operator sbyte(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (sbyte)mbr;
            else if (d is ManagedDecimal mde) return (sbyte)mde;
            else if (d is ManagedDouble mdo) return (sbyte)mdo;
            else return (sbyte)(ManagedFloat)d;
        }

        public static explicit operator short(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (short)mbr;
            else if (d is ManagedDecimal mde) return (short)mde;
            else if (d is ManagedDouble mdo) return (short)mdo;
            else return (short)(ManagedFloat)d;
        }

        public static explicit operator int(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (int)mbr;
            else if (d is ManagedDecimal mde) return (int)mde;
            else if (d is ManagedDouble mdo) return (int)mdo;
            else return (int)(ManagedFloat)d;
        }

        public static explicit operator long(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (long)mbr;
            else if (d is ManagedDecimal mde) return (long)mde;
            else if (d is ManagedDouble mdo) return (long)mdo;
            else return (long)(ManagedFloat)d;
        }

        public static explicit operator byte(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (byte)mbr;
            else if (d is ManagedDecimal mde) return (byte)mde;
            else if (d is ManagedDouble mdo) return (byte)mdo;
            else return (byte)(ManagedFloat)d;
        }

        public static explicit operator ushort(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (ushort)mbr;
            else if (d is ManagedDecimal mde) return (ushort)mde;
            else if (d is ManagedDouble mdo) return (ushort)mdo;
            else return (ushort)(ManagedFloat)d;
        }

        public static explicit operator uint(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (uint)mbr;
            else if (d is ManagedDecimal mde) return (uint)mde;
            else if (d is ManagedDouble mdo) return (uint)mdo;
            else return (uint)(ManagedFloat)d;
        }

        public static explicit operator ulong(ManagedRational d)
        {
            if (d is ManagedBigRational mbr) return (ulong)mbr;
            else if (d is ManagedDecimal mde) return (ulong)mde;
            else if (d is ManagedDouble mdo) return (ulong)mdo;
            else return (ulong)(ManagedFloat)d;
        }

        public static implicit operator ManagedRational(float b) => new ManagedFloat(b);
        public static implicit operator ManagedRational(double b) => new ManagedDouble(b);
        public static implicit operator ManagedRational(decimal b) => new ManagedDecimal(b);
        public static implicit operator ManagedRational(BigRational b) => new BigRational(b);
    }
}
