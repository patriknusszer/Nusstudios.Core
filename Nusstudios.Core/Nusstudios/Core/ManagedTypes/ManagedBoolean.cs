using Nusstudios.Core.Parsing.JSON;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedBoolean : JLeaf
    {
        public ManagedBoolean() { }
        public bool s;
        public ref bool Alias => ref s;
        public ManagedBoolean(bool s) =>  this.s = s;
        public static implicit operator bool(ManagedBoolean op) => op.s;
        public static implicit operator ManagedBoolean(bool op) => new ManagedBoolean(op);
    }
}
