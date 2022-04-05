using System;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    public class NumericKey : Key
    {
        public int key;
        public NumericKey(int key) : this(key, ".") { }
        public NumericKey(int key, string path_sep) { this.key = key; base.path_sep = path_sep; }
        public static implicit operator int(NumericKey op) => op.key;
        public static implicit operator NumericKey(int op) => new NumericKey(op);
        public static implicit operator ArrayKey(NumericKey op) => op.key;
        public static implicit operator NumericKey(ArrayKey op) => op.key;
        public override string ToComponent() => "(" + key + ")";
        public override Key Convert(string path_sep) => new NumericKey(key, base.path_sep);
        public override T ThrowOrGetRawKey<T>() => typeof(T) == typeof(Int32) ? (T)(object)key : throw new Exception();

        public override bool EqualsInRawAndType(Key k)
        {
            if (k == this) return true;
            else if (k is NumericKey nkey) return nkey.key == key;
            else return false;
        }
    }
}
