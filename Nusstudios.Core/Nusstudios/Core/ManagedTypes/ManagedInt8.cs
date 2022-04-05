using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedInt8 : ManagedSignedInteger
    {
        internal sbyte n;

        public ref sbyte Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator byte(ManagedInt8 op) => (byte)op.n;
        public static explicit operator ushort(ManagedInt8 op) => (ushort)op.n;
        public static explicit operator uint(ManagedInt8 op) => (uint)op.n;
        public static explicit operator ulong(ManagedInt8 op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedInt8(byte op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(ushort op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(uint op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(ulong op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(short op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(int op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(long op) => new ManagedInt8((sbyte)op);

        // always lossless implicit conversion to integral types
        public static implicit operator sbyte(ManagedInt8 op) => op.n;
        public static implicit operator short(ManagedInt8 op) => op.n;
        public static implicit operator int(ManagedInt8 op) => op.n;
        public static implicit operator long(ManagedInt8 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedInt8(sbyte op) => new ManagedInt8(op);

        // possibly lossy explicit conversions to floating-point types
        // none

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedInt8(float op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(double op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(decimal op) => new ManagedInt8((sbyte)op);
        public static explicit operator ManagedInt8(BigRational op) => new ManagedInt8((sbyte)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator float(ManagedInt8 op) => op.n;
        public static implicit operator double(ManagedInt8 op) => op.n;
        public static implicit operator decimal(ManagedInt8 op) => op.n;
        public static implicit operator BigRational(ManagedInt8 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedInt8(sbyte op)
        {
            this.n = op;
        }

        public ManagedInt8(ManagedNumber op)
        {
            this.n = (sbyte)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (sbyte)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (sbyte)op;
        }

        public override void Set(ManagedSignedInteger op)
        {
            this.n = (sbyte)op;
        }

        public void Set(sbyte op)
        {
            this.n = op;
        }

        public static ManagedInt8 operator +(ManagedInt8 operand) => new ManagedInt8(operand.n * 1);
        public static ManagedInt8 operator -(ManagedInt8 operand) => new ManagedInt8(operand.n * -1);
        public static ManagedInt8 operator ++(ManagedInt8 operand) => new ManagedInt8(operand.n + 1);
        public static ManagedInt8 operator --(ManagedInt8 operand) => new ManagedInt8(operand.n - 1);
        public static ManagedInt8 operator +(ManagedInt8 lhs, ManagedInt8 rhs) => new ManagedInt8(lhs.n + rhs.n);
        public static ManagedInt8 operator -(ManagedInt8 lhs, ManagedInt8 rhs) => new ManagedInt8(lhs.n - rhs.n);
        public static ManagedInt8 operator /(ManagedInt8 lhs, ManagedInt8 rhs) => new ManagedInt8(lhs.n / rhs.n);
        public static ManagedInt8 operator *(ManagedInt8 lhs, ManagedInt8 rhs) => new ManagedInt8(lhs.n * rhs.n);
        public static ManagedInt8 operator %(ManagedInt8 lhs, ManagedInt8 rhs) => new ManagedInt8(lhs.n % rhs.n);
    }
}
