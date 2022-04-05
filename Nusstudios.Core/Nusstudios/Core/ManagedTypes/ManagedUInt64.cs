using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedUInt64 : ManagedUnsignedInteger
    {
        internal ulong n;

        public ref ulong Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator byte(ManagedUInt64 op) => (byte)op.n;
        public static explicit operator ushort(ManagedUInt64 op) => (ushort)op.n;
        public static explicit operator uint(ManagedUInt64 op) => (uint)op.n;
        public static explicit operator sbyte(ManagedUInt64 op) => (sbyte)op.n;
        public static explicit operator short(ManagedUInt64 op) => (short)op.n;
        public static explicit operator int(ManagedUInt64 op) => (int)op.n;
        public static explicit operator long(ManagedUInt64 op) => (long)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedUInt64(sbyte op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(short op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(int op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(long op) => new ManagedUInt64((ulong)op);

        // always lossless implicit conversion to integral types
        public static implicit operator ulong(ManagedUInt64 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedUInt64(byte op) => new ManagedUInt64(op);
        public static implicit operator ManagedUInt64(ushort op) => new ManagedUInt64(op);
        public static implicit operator ManagedUInt64(uint op) => new ManagedUInt64(op);
        public static implicit operator ManagedUInt64(ulong op) => new ManagedUInt64(op);

        // possibly lossy explicit conversions to floating-point types
        public static explicit operator float(ManagedUInt64 op) => op.n;
        public static explicit operator double(ManagedUInt64 op) => op.n;

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedUInt64(float op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(double op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(decimal op) => new ManagedUInt64((ulong)op);
        public static explicit operator ManagedUInt64(BigRational op) => new ManagedUInt64((ulong)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator decimal(ManagedUInt64 op) => op.n;
        public static implicit operator BigRational(ManagedUInt64 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedUInt64(ulong op)
        {
            this.n = op;
        }

        public ManagedUInt64(ManagedNumber op)
        {
            this.n = (ulong)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (ulong)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (ulong)op;
        }

        public override void Set(ManagedUnsignedInteger op)
        {
            this.n = (ulong)op;
        }

        public void Set(ulong op)
        {
            this.n = op;
        }

        public static ManagedUInt64 operator +(ManagedUInt64 operand) => new ManagedUInt64(operand.n * 1);
        public static ManagedUInt64 operator -(ManagedUInt64 operand) => new ManagedUInt64((int)operand.n * -1);
        public static ManagedUInt64 operator ++(ManagedUInt64 operand) => new ManagedUInt64(operand.n + 1);
        public static ManagedUInt64 operator --(ManagedUInt64 operand) => new ManagedUInt64(operand.n - 1);
        public static ManagedUInt64 operator +(ManagedUInt64 lhs, ManagedUInt64 rhs) => new ManagedUInt64(lhs.n + rhs.n);
        public static ManagedUInt64 operator -(ManagedUInt64 lhs, ManagedUInt64 rhs) => new ManagedUInt64(lhs.n - rhs.n);
        public static ManagedUInt64 operator /(ManagedUInt64 lhs, ManagedUInt64 rhs) => new ManagedUInt64(lhs.n / rhs.n);
        public static ManagedUInt64 operator *(ManagedUInt64 lhs, ManagedUInt64 rhs) => new ManagedUInt64(lhs.n * rhs.n);
        public static ManagedUInt64 operator %(ManagedUInt64 lhs, ManagedUInt64 rhs) => new ManagedUInt64(lhs.n % rhs.n);
    }
}
