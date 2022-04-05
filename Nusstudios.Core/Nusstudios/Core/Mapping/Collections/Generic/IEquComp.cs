namespace Nusstudios.Core.Mapping.Collections.Generic
{
    public interface IEquComp<T>
    {
        public bool UpdateHashesDueToTablePrimeChange(int newModuloPrime);
        public ulong GetHashCode(T x);
        public bool Equals(T x, T y);
    }
}
