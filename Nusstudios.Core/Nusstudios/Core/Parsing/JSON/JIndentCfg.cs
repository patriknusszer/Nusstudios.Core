using System;

namespace Nusstudios.Core.Parsing.JSON
{
    public class JIndentCfg
    {
        private string indentString = "\t";
        private string stuffingAfterMemberKey = " ";
        private string stuffingBeforeMemberValue = " ";
        public int ObjectSpacing = 0;
        public int ArraySpacing = 0;
        public bool IndentOpenCurlyBrace = false;
        public bool IndentSpacing = false;

        private bool isStuffing(string str)
        {
            for (int i = -1; ++i < str.Length;) if (str[i] != '\n' && str[i] != '\r' && str[i] != '\t' && str[i] != ' ') return false;
            return true;
        }

        public string IndentString
        {
            get => indentString;
            set
            {
                if (isStuffing(value)) indentString = value;
                else throw new Exception("Provided value is not stuffing");
            }
        }

        public string StuffingAfterMemberKey
        {
            get => stuffingAfterMemberKey;
            set
            {
                if (isStuffing(value)) stuffingAfterMemberKey = value;
                else throw new Exception("Provided value is not stuffing");
            }
        }

        public string StuffingBeforeMemberValue
        {
            get => stuffingBeforeMemberValue;
            set
            {
                if (isStuffing(value)) stuffingBeforeMemberValue = value;
                else throw new Exception("Provided value is not stuffing");
            }
        }

        public JIndentCfg() { }

        public JIndentCfg(string indentString, string stuffingAfterMemberKey, string stuffingBeforeMemberValue, bool indentOpenCurlyBrace, int objectSpacing, int arraySpacing, bool indentSpacing)
        {
            this.indentString = indentString;
            this.stuffingAfterMemberKey = stuffingAfterMemberKey;
            this.stuffingBeforeMemberValue = stuffingBeforeMemberValue;
            IndentOpenCurlyBrace = indentOpenCurlyBrace;
            ObjectSpacing = objectSpacing;
            ArraySpacing = arraySpacing;
            IndentSpacing = indentSpacing;
        }
    }
}
