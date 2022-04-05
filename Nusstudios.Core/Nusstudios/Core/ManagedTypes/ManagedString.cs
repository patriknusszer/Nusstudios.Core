using Nusstudios.Core.Parsing.JSON;

namespace Nusstudios.Core.ManagedTypes
{
    public class ManagedString : JLeaf
    {
        public ManagedString() { }
        public string s;
        public ref string Alias => ref s;
        public ManagedString(string s) => this.s = s;
        public static implicit operator string(ManagedString op) => op.s;
        public static implicit operator ManagedString(string op) => new ManagedString(op);
    }
}
