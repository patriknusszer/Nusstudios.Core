using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedUInt32 : ManagedUnsignedInteger
    {
        internal uint n;

        public ref uint Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator byte(ManagedUInt32 op) => (byte)op.n;
        public static explicit operator ushort(ManagedUInt32 op) => (ushort)op.n;
        public static explicit operator sbyte(ManagedUInt32 op) => (sbyte)op.n;
        public static explicit operator short(ManagedUInt32 op) => (short)op.n;
        public static explicit operator int(ManagedUInt32 op) => (int)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedUInt32(ulong op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(sbyte op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(short op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(int op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(long op) => new ManagedUInt32((uint)op);

        // always lossless implicit conversion to integral types
        public static implicit operator uint(ManagedUInt32 op) => op.n;
        public static implicit operator ulong(ManagedUInt32 op) => op.n;
        public static implicit operator long(ManagedUInt32 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedUInt32(byte op) => new ManagedUInt32(op);
        public static implicit operator ManagedUInt32(ushort op) => new ManagedUInt32(op);
        public static implicit operator ManagedUInt32(uint op) => new ManagedUInt32(op);

        // possibly lossy explicit conversions to floating-point types
        public static explicit operator float(ManagedUInt32 op) => op.n;

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedUInt32(float op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(double op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(decimal op) => new ManagedUInt32((uint)op);
        public static explicit operator ManagedUInt32(BigRational op) => new ManagedUInt32((uint)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator double(ManagedUInt32 op) => op.n;
        public static implicit operator decimal(ManagedUInt32 op) => op.n;
        public static implicit operator BigRational(ManagedUInt32 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedUInt32(uint op)
        {
            this.n = op;
        }

        public ManagedUInt32(ManagedNumber op)
        {
            this.n = (uint)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (uint)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (uint)op;
        }

        public override void Set(ManagedUnsignedInteger op)
        {
            this.n = (uint)op;
        }

        public void Set(uint op)
        {
            this.n = op;
        }

        public static ManagedUInt32 operator +(ManagedUInt32 operand) => new ManagedUInt32(operand.n * 1);
        public static ManagedUInt32 operator -(ManagedUInt32 operand) => new ManagedUInt32(operand.n * -1);
        public static ManagedUInt32 operator ++(ManagedUInt32 operand) => new ManagedUInt32(operand.n + 1);
        public static ManagedUInt32 operator --(ManagedUInt32 operand) => new ManagedUInt32(operand.n - 1);
        public static ManagedUInt32 operator +(ManagedUInt32 lhs, ManagedUInt32 rhs) => new ManagedUInt32(lhs.n + rhs.n);
        public static ManagedUInt32 operator -(ManagedUInt32 lhs, ManagedUInt32 rhs) => new ManagedUInt32(lhs.n - rhs.n);
        public static ManagedUInt32 operator /(ManagedUInt32 lhs, ManagedUInt32 rhs) => new ManagedUInt32(lhs.n / rhs.n);
        public static ManagedUInt32 operator *(ManagedUInt32 lhs, ManagedUInt32 rhs) => new ManagedUInt32(lhs.n * rhs.n);
        public static ManagedUInt32 operator %(ManagedUInt32 lhs, ManagedUInt32 rhs) => new ManagedUInt32(lhs.n % rhs.n);
    }
}
