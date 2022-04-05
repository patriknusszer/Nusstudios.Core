namespace Nusstudios.Core.Parsing.JSON
{
    public abstract class JEntry
    {
        internal object key;
        internal JValue value;
        public JValue Value => value;
    }
}
