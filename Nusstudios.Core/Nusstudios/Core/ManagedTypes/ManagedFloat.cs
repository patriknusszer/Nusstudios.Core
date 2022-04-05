namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedFloat : ManagedRational
    {
        internal float n;

        public ref float Alias => ref n;

        // possibly lossy explicit conversions to smaller types, and from larger types
        public static explicit operator ManagedFloat(double op) => new ManagedFloat((float)op);

        // possibly lossy explicit conversions to and from unrelated floating-point types
        public static explicit operator decimal(ManagedFloat op) => (decimal)op.n;
        public static explicit operator ManagedFloat(decimal op) => new ManagedFloat((float)op);

        // possibly lossy explicit conversions to integral types
        public static explicit operator sbyte(ManagedFloat op) => (sbyte)op.n;
        public static explicit operator short(ManagedFloat op) => (short)op.n;
        public static explicit operator int(ManagedFloat op) => (int)op.n;
        public static explicit operator long(ManagedFloat op) => (long)op.n;
        public static explicit operator byte(ManagedFloat op) => (byte)op.n;
        public static explicit operator ushort(ManagedFloat op) => (ushort)op.n;
        public static explicit operator uint(ManagedFloat op) => (uint)op.n;
        public static explicit operator ulong(ManagedFloat op) => (ulong)op.n;

        // possibly lossy explicit conversions from integral types
        public static explicit operator ManagedFloat(int op) => new ManagedFloat(op);
        public static explicit operator ManagedFloat(long op) => new ManagedFloat(op);
        public static explicit operator ManagedFloat(uint op) => new ManagedFloat(op);
        public static explicit operator ManagedFloat(ulong op) => new ManagedFloat(op);

        // always lossless implicit convserions to integral types
        // none

        // always lossless implicit conversions from integral types
        public static implicit operator ManagedFloat(sbyte op) => new ManagedFloat(op);
        public static implicit operator ManagedFloat(short op) => new ManagedFloat(op);
        public static implicit operator ManagedFloat(byte op) => new ManagedFloat(op);
        public static implicit operator ManagedFloat(ushort op) => new ManagedFloat(op);

        // always lossless implicit conversion to larger/indentical, or from smaller/identical types
        public static implicit operator ManagedFloat(float op) => new ManagedFloat(op);
        public static implicit operator float(ManagedFloat op) => op.n;

        public ManagedFloat(float init)
        {
            this.n = init;
        }

        public ManagedFloat(ManagedNumber init)
        {
            this.n = (float)init;
        }

        public override void Set(ManagedNumber op)
        {
            this.n = (float)op;
        }

        public override void Set(ManagedRational op)
        {
            this.n = (float)op;
        }

        public void Set(float op)
        {
            this.n = op;
        }

        public static ManagedFloat operator +(ManagedFloat operand) => new ManagedFloat(operand.n * 1);
        public static ManagedFloat operator -(ManagedFloat operand) => new ManagedFloat(operand.n * -1);
        public static ManagedFloat operator ++(ManagedFloat operand) => new ManagedFloat(operand.n + 1);
        public static ManagedFloat operator --(ManagedFloat operand) => new ManagedFloat(operand.n - 1);
        public static ManagedFloat operator +(ManagedFloat lhs, ManagedFloat rhs) => new ManagedFloat(lhs.n + rhs.n);
        public static ManagedFloat operator -(ManagedFloat lhs, ManagedFloat rhs) => new ManagedFloat(lhs.n - rhs.n);
        public static ManagedFloat operator /(ManagedFloat lhs, ManagedFloat rhs) => new ManagedFloat(lhs.n / rhs.n);
        public static ManagedFloat operator *(ManagedFloat lhs, ManagedFloat rhs) => new ManagedFloat(lhs.n * rhs.n);
        public static ManagedFloat operator %(ManagedFloat lhs, ManagedFloat rhs) => new ManagedFloat(lhs.n % rhs.n);
    }
}
