using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedUInt8 : ManagedUnsignedInteger
    {
        internal byte n;

        public ref byte Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedUInt8 op) => (sbyte)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedUInt8(ushort op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(uint op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(ulong op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(sbyte op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(short op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(int op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(long op) => new ManagedUInt8((byte)op);

        // always lossless implicit conversion to integral types
        public static implicit operator byte(ManagedUInt8 op) => op.n;
        public static implicit operator ushort(ManagedUInt8 op) => op.n;
        public static implicit operator uint(ManagedUInt8 op) => op.n;
        public static implicit operator ulong(ManagedUInt8 op) => op.n;
        public static implicit operator short(ManagedUInt8 op) => op.n;
        public static implicit operator int(ManagedUInt8 op) => op.n;
        public static implicit operator long(ManagedUInt8 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedUInt8(byte op) => new ManagedUInt8(op);

        // possibly lossy explicit conversions to floating-point types
        // none

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedUInt8(float op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(double op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(decimal op) => new ManagedUInt8((byte)op);
        public static explicit operator ManagedUInt8(BigRational op) => new ManagedUInt8((byte)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator float(ManagedUInt8 op) => op.n;
        public static implicit operator double(ManagedUInt8 op) => op.n;
        public static implicit operator decimal(ManagedUInt8 op) => op.n;
        public static implicit operator BigRational(ManagedUInt8 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedUInt8(byte op)
        {
            this.n = op;
        }

        public ManagedUInt8(ManagedNumber op)
        {
            this.n = (byte)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (byte)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (byte)op;
        }

        public override void Set(ManagedUnsignedInteger op)
        {
            this.n = (byte)op;
        }

        public void Set(byte op)
        {
            this.n = op;
        }

        public static ManagedUInt8 operator +(ManagedUInt8 operand) => new ManagedUInt8(operand.n * 1);
        public static ManagedUInt8 operator -(ManagedUInt8 operand) => new ManagedUInt8(operand.n * -1);
        public static ManagedUInt8 operator ++(ManagedUInt8 operand) => new ManagedUInt8(operand.n + 1);
        public static ManagedUInt8 operator --(ManagedUInt8 operand) => new ManagedUInt8(operand.n - 1);
        public static ManagedUInt8 operator +(ManagedUInt8 lhs, ManagedUInt8 rhs) => new ManagedUInt8(lhs.n + rhs.n);
        public static ManagedUInt8 operator -(ManagedUInt8 lhs, ManagedUInt8 rhs) => new ManagedUInt8(lhs.n - rhs.n);
        public static ManagedUInt8 operator /(ManagedUInt8 lhs, ManagedUInt8 rhs) => new ManagedUInt8(lhs.n / rhs.n);
        public static ManagedUInt8 operator *(ManagedUInt8 lhs, ManagedUInt8 rhs) => new ManagedUInt8(lhs.n * rhs.n);
        public static ManagedUInt8 operator %(ManagedUInt8 lhs, ManagedUInt8 rhs) => new ManagedUInt8(lhs.n % rhs.n);
    }
}
