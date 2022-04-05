using System;
using System.Collections.Generic;

namespace Nusstudios.Core {
    public class StringUtil
    {
        public static string[] SplitInParts(String str, int perNChars)
        {
            List<string> parts = new List<string>();
            for (var i = 0; i < str.Length; i += perNChars) parts.Add(str.Substring(i, Math.Min(perNChars, str.Length - i)));
            return parts.ToArray();
        }

        public static string AppendChar(string str, string fillChar, int amount)
        {
            for (int i = 0; i < amount; i++) str += fillChar;
            return str;
        }

        public static String PrependChar(String str, String fillChar, int amount)
        {
            for (int i = 0; i < amount; i++) str = fillChar + str;
            return str;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static String TrimStart(string n, int amount) => n.Substring(amount);
        static String TrimEnd(String n, int amount) => n.Substring(0, n.Length - amount);
    }
}