using System;

namespace Nusstudios.Core.Mapping.DynamicObject
{
    public class ArrayKey : Key
    {
        public int key;
        public ArrayKey(int key) : this(key, ".") { }
        public ArrayKey(int key, string path_sep) { this.key = key; base.path_sep = path_sep; }
        public static implicit operator int(ArrayKey op) => op.key;
        public static implicit operator ArrayKey(int op) => new ArrayKey(op);
        public override string ToComponent() => "[" + key + "]";
        public override Key Convert(string path_sep) => new ArrayKey(key, base.path_sep);
        public override T ThrowOrGetRawKey<T>() => typeof(T) == typeof(Int32) ? (T)(object)key : throw new Exception("ADGBJAKFgkgsg");

        public override bool EqualsInRawAndType(Key k)
        {
            if (k == this) return true;
            else if (k is ArrayKey akey) return akey.key == key;
            else return false;
        }
    }
}
