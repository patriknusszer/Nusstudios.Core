using System;

namespace Nusstudios.Core.Parsing.JSON
{
    public abstract class JLeaf : JValue
    {
        public override JValue this[object key] { get => throw new Exception("Not a JContainer"); set => throw new Exception("Not a JContainer"); }
    }
}
