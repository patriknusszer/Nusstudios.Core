namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedDouble : ManagedRational
    {
        internal double n;

        public ref double Alias => ref n;

        // possibly lossy explicit conversions to smaller types, and from larger types
        public static explicit operator float(ManagedDouble op) => (float)op.n;

        // possibly lossy explicit conversions to and from unrelated floating-point types
        public static explicit operator decimal(ManagedDouble op) => (decimal)op.n;
        public static explicit operator ManagedDouble(decimal op) => new ManagedDouble((double)op);

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedDouble op) => (sbyte)op.n;
        public static explicit operator short(ManagedDouble op) => (short)op.n;
        public static explicit operator int(ManagedDouble op) => (int)op.n;
        public static explicit operator long(ManagedDouble op) => (long)op.n;
        public static explicit operator byte(ManagedDouble op) => (byte)op.n;
        public static explicit operator ushort(ManagedDouble op) => (ushort)op.n;
        public static explicit operator uint(ManagedDouble op) => (uint)op.n;
        public static explicit operator ulong(ManagedDouble op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedDouble(long op) => new ManagedDouble(op);
        public static explicit operator ManagedDouble(ulong op) => new ManagedDouble(op);

        // always lossless implicit convserions to integral types
        // none

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedDouble(sbyte op) => new ManagedDouble(op);
        public static implicit operator ManagedDouble(short op) => new ManagedDouble(op);
        public static implicit operator ManagedDouble(int op) => new ManagedDouble(op);
        public static implicit operator ManagedDouble(byte op) => new ManagedDouble(op);
        public static implicit operator ManagedDouble(ushort op) => new ManagedDouble(op);
        public static implicit operator ManagedDouble(uint op) => new ManagedDouble(op);


        // always lossless implicit conversion to larger/indentical, or from smaller/identical types
        public static implicit operator ManagedDouble(double op) => new ManagedDouble(op);
        public static implicit operator double(ManagedDouble op) => op.n;

        public ManagedDouble(double init)
        {
            this.n = init;
        }

        public ManagedDouble(ManagedNumber init)
        {
            this.n = (double)init;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (double)op;
        }

        public override void Set(ManagedRational op)
        {
            this.n = (double)op;
        }

        public void Set(double op)
        {
            this.n = op;
        }

        public static ManagedDouble operator +(ManagedDouble operand) => new ManagedDouble(operand.n * 1);
        public static ManagedDouble operator -(ManagedDouble operand) => new ManagedDouble(operand.n * -1);
        public static ManagedDouble operator ++(ManagedDouble operand) => new ManagedDouble(operand.n + 1);
        public static ManagedDouble operator --(ManagedDouble operand) => new ManagedDouble(operand.n - 1);
        public static ManagedDouble operator +(ManagedDouble lhs, ManagedDouble rhs) => new ManagedDouble(lhs.n + rhs.n);
        public static ManagedDouble operator -(ManagedDouble lhs, ManagedDouble rhs) => new ManagedDouble(lhs.n - rhs.n);
        public static ManagedDouble operator /(ManagedDouble lhs, ManagedDouble rhs) => new ManagedDouble(lhs.n / rhs.n);
        public static ManagedDouble operator *(ManagedDouble lhs, ManagedDouble rhs) => new ManagedDouble(lhs.n * rhs.n);
        public static ManagedDouble operator %(ManagedDouble lhs, ManagedDouble rhs) => new ManagedDouble(lhs.n % rhs.n);
    }
}
