namespace Nusstudios.Core.Mapping.DynamicObject
{
    public abstract class Key
    {
        public string path_sep;
        public abstract string ToComponent();
        public bool IsCompatibleWith(string path_sep) => this.path_sep == path_sep;
        public abstract Key Convert(string path_sep);
        public abstract T ThrowOrGetRawKey<T>();
        public abstract bool EqualsInRawAndType(Key k);
    }
}
