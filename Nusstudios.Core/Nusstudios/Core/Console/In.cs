using System;

namespace Nusstudios.Core.Console
{
    public class In
    {
        private ConsoleManager cmgr;

        public In(ConsoleManager manager)
        {
            cmgr = manager;
        }

        public int ReadKey(out string input)
        {
            return cmgr.RegisteringRead(out input, ConsoleManager.ReadType.ReadKey);
        }

        public int ReadLine(out string input)
        {
            return cmgr.RegisteringRead(out input, ConsoleManager.ReadType.ReadLine);
        }
    }
}
