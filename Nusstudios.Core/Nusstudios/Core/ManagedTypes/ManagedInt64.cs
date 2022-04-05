using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedInt64 : ManagedSignedInteger
    {
        internal long n;

        public ref long Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedInt64 op) => (sbyte)op.n;
        public static explicit operator short(ManagedInt64 op) => (short)op.n;
        public static explicit operator int(ManagedInt64 op) => (int)op.n;
        public static explicit operator byte(ManagedInt64 op) => (byte)op.n;
        public static explicit operator ushort(ManagedInt64 op) => (ushort)op.n;
        public static explicit operator uint(ManagedInt64 op) => (uint)op.n;
        public static explicit operator ulong(ManagedInt64 op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedInt64(ulong op) => new ManagedInt64((int)op);

        // always lossless implicit conversion to integral types
        public static implicit operator long(ManagedInt64 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedInt64(sbyte op) => new ManagedInt64(op);
        public static implicit operator ManagedInt64(short op) => new ManagedInt64(op);
        public static implicit operator ManagedInt64(int op) => new ManagedInt64(op);
        public static implicit operator ManagedInt64(long op) => new ManagedInt64((int)op);
        public static implicit operator ManagedInt64(byte op) => new ManagedInt64(op);
        public static implicit operator ManagedInt64(ushort op) => new ManagedInt64(op);
        public static implicit operator ManagedInt64(uint op) => new ManagedInt64((int)op);

        // possibly lossy explicit conversions to floating-point types
        public static explicit operator float(ManagedInt64 op) => op.n;
        public static explicit operator double(ManagedInt64 op) => op.n;

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedInt64(float op) => new ManagedInt64((int)op);
        public static explicit operator ManagedInt64(double op) => new ManagedInt64((int)op);
        public static explicit operator ManagedInt64(decimal op) => new ManagedInt64((int)op);
        public static explicit operator ManagedInt64(BigRational op) => new ManagedInt64((int)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator decimal(ManagedInt64 op) => op.n;
        public static implicit operator BigRational(ManagedInt64 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedInt64(long op)
        {
            this.n = op;
        }

        public ManagedInt64(ManagedInteger op)
        {
            this.n = (long)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (long)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (long)op;
        }

        public override void Set(ManagedSignedInteger op)
        {
            this.n = (long)op;
        }

        public void Set(long op)
        {
            this.n = op;
        }

        public static ManagedInt64 operator +(ManagedInt64 operand) => new ManagedInt64(operand.n * 1);
        public static ManagedInt64 operator -(ManagedInt64 operand) => new ManagedInt64(operand.n * -1);
        public static ManagedInt64 operator ++(ManagedInt64 operand) => new ManagedInt64(operand.n + 1);
        public static ManagedInt64 operator --(ManagedInt64 operand) => new ManagedInt64(operand.n - 1);
        public static ManagedInt64 operator +(ManagedInt64 lhs, ManagedInt64 rhs) => new ManagedInt64(lhs.n + rhs.n);
        public static ManagedInt64 operator -(ManagedInt64 lhs, ManagedInt64 rhs) => new ManagedInt64(lhs.n - rhs.n);
        public static ManagedInt64 operator /(ManagedInt64 lhs, ManagedInt64 rhs) => new ManagedInt64(lhs.n / rhs.n);
        public static ManagedInt64 operator *(ManagedInt64 lhs, ManagedInt64 rhs) => new ManagedInt64(lhs.n * rhs.n);
        public static ManagedInt64 operator %(ManagedInt64 lhs, ManagedInt64 rhs) => new ManagedInt64(lhs.n % rhs.n);
    }
}
