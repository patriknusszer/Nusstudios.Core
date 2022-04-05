using System.Collections.Generic;
using System;

namespace Nusstudios.Core {
    public class BaseConverter
    {
        public static string Convert(string number, int fromBase, int toBase, int precision1, int precision2)
        {
            string value = GetValueFromBase(number, fromBase, precision1);
            string basenumber = GetBaseFromValue(value, toBase, precision2);
            return basenumber;
        }

        public static string GetValueFromBase(string number, int numberBase, int precision)
        {
            bool isNegative = false;

            if (StringMath.is_negative(number))
            {
                number = StringMath.switch_sign(number);
                isNegative = true;
            }

            string integer_part = StringMath.get_integer_part(number);
            string fraction_part = StringMath.get_fraction_part(number);
            char[] integer_part_digits = integer_part.ToCharArray();
            char[] fraction_part_digits = fraction_part.ToCharArray();
            string value = "0";

            for (int i = integer_part_digits.Length - 1, e = 0; i >= 0; i--, e++)
            {
                int charvalue = System.Convert.ToInt32(GetValueFromBaseDigit(integer_part_digits[i]));
                value = StringMath.add(new List<string> { value, StringMath.multiply(new List<string> { charvalue.ToString(), StringMath.exponentiate(numberBase.ToString(), e.ToString(), 0) }) });
            }

            for (int i = 0, e = -1; i < fraction_part_digits.Length; i++, e--)
            {
                int charvalue = System.Convert.ToInt32(GetValueFromBaseDigit(fraction_part_digits[i]));
                value = StringMath.add(new List<string> { value, StringMath.multiply(new List<string> { charvalue.ToString(), StringMath.exponentiate(numberBase.ToString(), e.ToString(), 0) }) });
            }

            if (isNegative)
            {
                value = StringMath.switch_sign(value);
            }

            return value;
        }

        public static double GetValueFromBaseDigit(char digit)
        {
            if (char.IsDigit(digit))
            {
                return char.GetNumericValue(digit);
            }
            else
            {
                switch (digit)
                {
                    case 'a':
                        return 10;
                    case 'b':
                        return 11;
                    case 'c':
                        return 12;
                    case 'd':
                        return 13;
                    case 'e':
                        return 14;
                    case 'f':
                        return 15;
                }

                throw new Exception();
            }
        }

        public static char GetBaseDigitForValue(int value)
        {
            if (value < 10)
            {
                return value.ToString()[0];
            }
            else
            {
                switch (value)
                {
                    case 10:
                        return 'a';
                    case 11:
                        return 'b';
                    case 12:
                        return 'c';
                    case 13:
                        return 'd';
                    case 14:
                        return 'e';
                    case 15:
                        return 'f';
                }

                throw new Exception();
            }
        }

        public static string GetBaseFromValue(string value, int numberBase, int precision)
        {
            bool isNegative = false;

            if (StringMath.is_greater("0", value))
            {
                value = StringMath.switch_sign(value);
                isNegative = true;
            }

            string baseNumber = "";
            string integer_part = StringMath.get_integer_part(value);
            string fraction_part = "0." + StringMath.get_fraction_part(value);
            string timesForNextDigit = integer_part;
            string remainderForCurrentDigit;
            char c;

            do {
                remainderForCurrentDigit = StringMath.remainder(timesForNextDigit, numberBase.ToString());
                timesForNextDigit = StringMath.divide(new List<string> { timesForNextDigit, numberBase.ToString() }, 0);
                c = GetBaseDigitForValue(System.Convert.ToInt32(remainderForCurrentDigit));
                baseNumber = c + baseNumber;
            } while (!StringMath.are_equal(timesForNextDigit, "0"));

            int _precision = 0;


            for (int e = 0; true; e--) {
                if (_precision == precision)
                {
                    break;
                }

                if (StringMath.are_equal(fraction_part, "0")) {
                    break;
                }
                else if (e == 0) {
                    baseNumber += ".";
                }

                fraction_part = StringMath.multiply(new List<string> { fraction_part, numberBase.ToString() });
                string digitValue = StringMath.get_integer_part(fraction_part);

                if (StringMath.is_greater_or_equal(digitValue, "1"))
                {
                    fraction_part = StringMath.subtract(new List<string> { fraction_part, digitValue });
                    char digit = GetBaseDigitForValue(System.Convert.ToInt32(digitValue));
                    baseNumber += digit;
                }
                else
                {
                    baseNumber += "0";
                }

                _precision++;
            }

            if (isNegative) {
                baseNumber = StringMath.switch_sign(baseNumber);
            }

            return StringMath.beautify_number(baseNumber);
        }
    }
}