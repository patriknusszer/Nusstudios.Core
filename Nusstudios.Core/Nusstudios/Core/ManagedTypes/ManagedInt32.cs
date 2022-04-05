using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedInt32 : ManagedSignedInteger
    {
        internal int n;

        public ref int Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedInt32 op) => (sbyte)op.n;
        public static explicit operator short(ManagedInt32 op) => (short)op.n;
        public static explicit operator byte(ManagedInt32 op) => (byte)op.n;
        public static explicit operator ushort(ManagedInt32 op) => (ushort)op.n;
        public static explicit operator uint(ManagedInt32 op) => (uint)op.n;
        public static explicit operator ulong(ManagedInt32 op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedInt32(uint op) => new ManagedInt32((int)op);
        public static explicit operator ManagedInt32(ulong op) => new ManagedInt32((int)op);
        public static explicit operator ManagedInt32(long op) => new ManagedInt32((int)op);

        // always lossless implicit conversion to integral types
        public static implicit operator int(ManagedInt32 op) => op.n;
        public static implicit operator long(ManagedInt32 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedInt32(sbyte op) => new ManagedInt32(op);
        public static implicit operator ManagedInt32(short op) => new ManagedInt32(op);
        public static implicit operator ManagedInt32(int op) => new ManagedInt32(op);
        public static implicit operator ManagedInt32(byte op) => new ManagedInt32(op);
        public static implicit operator ManagedInt32(ushort op) => new ManagedInt32(op);

        // possibly lossy explicit conversions to floating-point types
        public static explicit operator float(ManagedInt32 op) => op.n;

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedInt32(float op) => new ManagedInt32((int)op);
        public static explicit operator ManagedInt32(double op) => new ManagedInt32((int)op);
        public static explicit operator ManagedInt32(decimal op) => new ManagedInt32((int)op);
        public static explicit operator ManagedInt32(BigRational op) => new ManagedInt32((int)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator double(ManagedInt32 op) => op.n;
        public static implicit operator decimal(ManagedInt32 op) => op.n;
        public static implicit operator BigRational(ManagedInt32 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedInt32(int op)
        {
            this.n = op;
        }

        public ManagedInt32(ManagedNumber op)
        {
            this.n = (int)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (int)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (int)op;
        }

        public override void Set(ManagedSignedInteger op)
        {
            this.n = (int)op;
        }

        public void Set(int op)
        {
            this.n = op;
        }

        public static ManagedInt32 operator +(ManagedInt32 operand) => new ManagedInt32(operand.n * 1);
        public static ManagedInt32 operator -(ManagedInt32 operand) => new ManagedInt32(operand.n * -1);
        public static ManagedInt32 operator ++(ManagedInt32 operand) => new ManagedInt32(operand.n + 1);
        public static ManagedInt32 operator --(ManagedInt32 operand) => new ManagedInt32(operand.n - 1);
        public static ManagedInt32 operator +(ManagedInt32 lhs, ManagedInt32 rhs) => new ManagedInt32(lhs.n + rhs.n);
        public static ManagedInt32 operator -(ManagedInt32 lhs, ManagedInt32 rhs) => new ManagedInt32(lhs.n - rhs.n);
        public static ManagedInt32 operator /(ManagedInt32 lhs, ManagedInt32 rhs) => new ManagedInt32(lhs.n / rhs.n);
        public static ManagedInt32 operator *(ManagedInt32 lhs, ManagedInt32 rhs) => new ManagedInt32(lhs.n * rhs.n);
        public static ManagedInt32 operator %(ManagedInt32 lhs, ManagedInt32 rhs) => new ManagedInt32(lhs.n % rhs.n);
    }
}
