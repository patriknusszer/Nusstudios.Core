namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedDecimal : ManagedRational
    {
        internal decimal n;

        public ref decimal Alias => ref n;

        // possibly lossy explicit conversions to smaller types, and from larger types
        // none

        // possibly lossy explicit conversions to and from unrelated floating-point types
        public static explicit operator double(ManagedDecimal op) => (double)op.n;
        public static explicit operator ManagedDecimal(double op) => new ManagedDecimal((decimal)op);
        public static explicit operator float(ManagedDecimal op) => (float)op.n;
        public static explicit operator ManagedDecimal(float op) => new ManagedDecimal((decimal)op);

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedDecimal op) => (sbyte)op.n;
        public static explicit operator short(ManagedDecimal op) => (short)op.n;
        public static explicit operator int(ManagedDecimal op) => (int)op.n;
        public static explicit operator long(ManagedDecimal op) => (long)op.n;
        public static explicit operator byte(ManagedDecimal op) => (byte)op.n;
        public static explicit operator ushort(ManagedDecimal op) => (ushort)op.n;
        public static explicit operator uint(ManagedDecimal op) => (uint)op.n;
        public static explicit operator ulong(ManagedDecimal op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        // none

        // always lossless implicit convserions to integral types
        // none

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedDecimal(sbyte op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(short op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(int op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(long op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(byte op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(ushort op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(uint op) => new ManagedDecimal(op);
        public static implicit operator ManagedDecimal(ulong op) => new ManagedDecimal(op);

        // always lossless implicit conversion to larger/indentical, or from smaller/identical types
        public static implicit operator ManagedDecimal(decimal op) => new ManagedDecimal(op);
        public static implicit operator decimal(ManagedDecimal op) => op.n;

        public ManagedDecimal(decimal op)
        {
            this.n = op;
        }

        public ManagedDecimal(ManagedNumber op)
        {
            this.n = (decimal)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (decimal)op;
        }

        public override void Set(ManagedRational op)
        {
            this.n = (decimal)op;
        }

        public void Set(decimal op)
        {
            this.n = op;
        }

        public static ManagedDecimal operator +(ManagedDecimal operand) => new ManagedDecimal(operand.n * 1);
        public static ManagedDecimal operator -(ManagedDecimal operand) => new ManagedDecimal(operand.n * -1);
        public static ManagedDecimal operator ++(ManagedDecimal operand) => new ManagedDecimal(operand.n + 1);
        public static ManagedDecimal operator --(ManagedDecimal operand) => new ManagedDecimal(operand.n - 1);
        public static ManagedDecimal operator +(ManagedDecimal lhs, ManagedDecimal rhs) => new ManagedDecimal(lhs.n + rhs.n);
        public static ManagedDecimal operator -(ManagedDecimal lhs, ManagedDecimal rhs) => new ManagedDecimal(lhs.n - rhs.n);
        public static ManagedDecimal operator /(ManagedDecimal lhs, ManagedDecimal rhs) => new ManagedDecimal(lhs.n / rhs.n);
        public static ManagedDecimal operator *(ManagedDecimal lhs, ManagedDecimal rhs) => new ManagedDecimal(lhs.n * rhs.n);
        public static ManagedDecimal operator %(ManagedDecimal lhs, ManagedDecimal rhs) => new ManagedDecimal(lhs.n % rhs.n);
    }
}
