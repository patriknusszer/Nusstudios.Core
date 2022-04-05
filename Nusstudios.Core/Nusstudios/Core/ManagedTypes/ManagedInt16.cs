using Nusstudios.Core.UnmanagedTypes;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedInt16 : ManagedSignedInteger
    {
        internal short n;

        public ref short Alias => ref n;

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedInt16 op) => (sbyte)op.n;
        public static explicit operator byte(ManagedInt16 op) => (byte)op.n;
        public static explicit operator ushort(ManagedInt16 op) => (ushort)op.n;
        public static explicit operator uint(ManagedInt16 op) => (uint)op.n;
        public static explicit operator ulong(ManagedInt16 op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static implicit operator ManagedInt16(ushort op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(uint op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(ulong op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(int op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(long op) => new ManagedInt16((short)op);

        // always lossless implicit conversion to integral types
        public static implicit operator short(ManagedInt16 op) => op.n;
        public static implicit operator int(ManagedInt16 op) => op.n;
        public static implicit operator long(ManagedInt16 op) => op.n;

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedInt16(sbyte op) => new ManagedInt16(op);
        public static implicit operator ManagedInt16(short op) => new ManagedInt16(op);
        public static implicit operator ManagedInt16(byte op) => new ManagedInt16(op);

        // possibly lossy explicit conversions to floating-point types
        // none

        // possibly lossy explicit conversions from floating-point types
        public static explicit operator ManagedInt16(float op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(double op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(decimal op) => new ManagedInt16((short)op);
        public static explicit operator ManagedInt16(BigRational op) => new ManagedInt16((short)op);

        // always lossless implicit conversions to floating-point types
        public static implicit operator float(ManagedInt16 op) => op.n;
        public static implicit operator double(ManagedInt16 op) => op.n;
        public static implicit operator decimal(ManagedInt16 op) => op.n;
        public static implicit operator BigRational(ManagedInt16 op) => op.n;

        // always lossless implicit conversions from floating-point types
        //none

        public ManagedInt16(short op)
        {
            this.n = op;
        }

        public ManagedInt16(ManagedNumber op)
        {
            this.n = (short)op;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (short)op;
        }

        public override void Set(ManagedInteger op)
        {
            this.n = (short)op;
        }

        public override void Set(ManagedSignedInteger op)
        {
            this.n = (short)op;
        }

        public void Set(short op)
        {
            this.n = op;
        }

        public static ManagedInt16 operator +(ManagedInt16 operand) => new ManagedInt16(operand.n * 1);
        public static ManagedInt16 operator -(ManagedInt16 operand) => new ManagedInt16(operand.n * -1);
        public static ManagedInt16 operator ++(ManagedInt16 operand) => new ManagedInt16(operand.n + 1);
        public static ManagedInt16 operator --(ManagedInt16 operand) => new ManagedInt16(operand.n - 1);
        public static ManagedInt16 operator +(ManagedInt16 lhs, ManagedInt16 rhs) => new ManagedInt16(lhs.n + rhs.n);
        public static ManagedInt16 operator -(ManagedInt16 lhs, ManagedInt16 rhs) => new ManagedInt16(lhs.n - rhs.n);
        public static ManagedInt16 operator /(ManagedInt16 lhs, ManagedInt16 rhs) => new ManagedInt16(lhs.n / rhs.n);
        public static ManagedInt16 operator *(ManagedInt16 lhs, ManagedInt16 rhs) => new ManagedInt16(lhs.n * rhs.n);
        public static ManagedInt16 operator %(ManagedInt16 lhs, ManagedInt16 rhs) => new ManagedInt16(lhs.n % rhs.n);
    }
}
