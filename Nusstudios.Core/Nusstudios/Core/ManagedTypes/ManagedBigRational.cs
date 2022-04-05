using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedBigRational : ManagedRational
    {
        internal BigRational n;

        public ref BigRational Alias => ref n;

        // possibly lossy explicit conversions from floating-point types
        // none

        // possibly lossy explicit conversions to floating-point types
        public static explicit operator float(ManagedBigRational op) => (float)op.n;
        public static explicit operator double(ManagedBigRational op) => (double)op.n;
        public static explicit operator decimal(ManagedBigRational op) => (decimal)op.n;
        public static explicit operator BigRational(ManagedBigRational op) => op.n;

        // always lossless implicit conversions to floating-point types
        // none

        // always lossless implicit conversions from floating-point types
        public static implicit operator ManagedBigRational(float op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(double op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(decimal op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(BigRational op) => new ManagedBigRational(op);

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedBigRational op) => (sbyte)op.n;
        public static explicit operator short(ManagedBigRational op) => (short)op.n;
        public static explicit operator int(ManagedBigRational op) => (int)op.n;
        public static explicit operator long(ManagedBigRational op) => (long)op.n;
        public static explicit operator byte(ManagedBigRational op) => (byte)op.n;
        public static explicit operator ushort(ManagedBigRational op) => (ushort)op.n;
        public static explicit operator uint(ManagedBigRational op) => (uint)op.n;
        public static explicit operator ulong(ManagedBigRational op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        // none

        // always lossless implicit convserions to integral types
        // none

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedBigRational(sbyte op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(short op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(int op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(long op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(byte op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(ushort op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(uint op) => new ManagedBigRational(op);
        public static implicit operator ManagedBigRational(ulong op) => new ManagedBigRational(op);

        public ManagedBigRational(BigRational op)
        {
            this.n = op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (BigRational)op;
        }

        public override void Set(ManagedRational op)
        {
            this.n = (BigRational)op;
        }
    }
}
