using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedUInt16 : ManagedUnsignedInteger
    {
        internal ushort n;

        public ref ushort Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator byte(ManagedUInt16 op) => (byte)op.n;
        public static explicit operator sbyte(ManagedUInt16 op) => (sbyte)op.n;
        public static explicit operator short(ManagedUInt16 op) => (short)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedUInt16(uint op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(ulong op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(sbyte op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(short op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(int op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(long op) => new ManagedUInt16((ushort)op);

        // always lossless implicit conversion to integral types
        public static implicit operator ushort(ManagedUInt16 op) => op.n;
        public static implicit operator uint(ManagedUInt16 op) => op.n;
        public static implicit operator ulong(ManagedUInt16 op) => op.n;
        public static implicit operator int(ManagedUInt16 op) => op.n;
        public static implicit operator long(ManagedUInt16 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedUInt16(byte op) => new ManagedUInt16(op);
        public static implicit operator ManagedUInt16(ushort op) => new ManagedUInt16(op);

        // possibly lossy explicit conversions to floating-point types
        // none

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedUInt16(float op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(double op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(decimal op) => new ManagedUInt16((ushort)op);
        public static explicit operator ManagedUInt16(BigRational op) => new ManagedUInt16((ushort)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator float(ManagedUInt16 op) => op.n;
        public static implicit operator double(ManagedUInt16 op) => op.n;
        public static implicit operator decimal(ManagedUInt16 op) => op.n;
        public static implicit operator BigRational(ManagedUInt16 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedUInt16(ushort op)
        {
            this.n = op;
        }

        public ManagedUInt16(ManagedNumber op)
        {
            this.n = (ushort)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (ushort)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (ushort)op;
        }

        public override void Set(ManagedUnsignedInteger op)
        {
            this.n = (ushort)op;
        }

        public void Set(ushort op)
        {
            this.n = op;
        }

        public static ManagedUInt16 operator +(ManagedUInt16 operand) => new ManagedUInt16(operand.n * 1);
        public static ManagedUInt16 operator -(ManagedUInt16 operand) => new ManagedUInt16(operand.n * -1);
        public static ManagedUInt16 operator ++(ManagedUInt16 operand) => new ManagedUInt16(operand.n + 1);
        public static ManagedUInt16 operator --(ManagedUInt16 operand) => new ManagedUInt16(operand.n - 1);
        public static ManagedUInt16 operator +(ManagedUInt16 lhs, ManagedUInt16 rhs) => new ManagedUInt16(lhs.n + rhs.n);
        public static ManagedUInt16 operator -(ManagedUInt16 lhs, ManagedUInt16 rhs) => new ManagedUInt16(lhs.n - rhs.n);
        public static ManagedUInt16 operator /(ManagedUInt16 lhs, ManagedUInt16 rhs) => new ManagedUInt16(lhs.n / rhs.n);
        public static ManagedUInt16 operator *(ManagedUInt16 lhs, ManagedUInt16 rhs) => new ManagedUInt16(lhs.n * rhs.n);
        public static ManagedUInt16 operator %(ManagedUInt16 lhs, ManagedUInt16 rhs) => new ManagedUInt16(lhs.n % rhs.n);
    }
}
