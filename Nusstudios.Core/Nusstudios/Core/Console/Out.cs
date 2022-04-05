using System;

namespace Nusstudios.Core.Console
{
    public class Out
    {
        private ConsoleManager cmgr;

        public Out(ConsoleManager manager)
        {
            cmgr = manager;
        }

        public int Write(string text)
        {
            return cmgr.RegisteringWrite(text, false);
        }

        public int WriteLine(string text)
        {
            return cmgr.RegisteringWrite(text, true);
        }

        public int WriteLine()
        {
            return cmgr.RegisteringWrite("", true);
        }
    }
}
