using System;
using System.Collections.Generic;

namespace Nusstudios.Core
{
    public static class StringMath
    {
        public static bool is_fraction(string n) => n.IndexOf('.') != -1;
        public static bool is_integer(string n) => n.IndexOf('.') == -1;
        static string to_fraction(string n) => is_integer(n) ? n + ".0" : n;
        public static string get_fraction_part(string n) => is_integer(n) ? "0" : n.Split('.')[1];
        public static string get_integer_part(string n) => is_fraction(n) ? n.Split('.')[0] : n;
        static string trim_start(string n, int amount) => n.Substring(amount);
        static string trim_end(string n, int amount) => n.Substring(0, n.Length - amount);
        public static bool is_negative(string n) => n[0] == '-';
        public static bool is_positive(string n) => !(n[0] == '-');
        static bool is_longer(string n, string than) => n.Length > than.Length;
        public static string get_abs(string n) => is_negative(n) ? switch_sign(n) : n;
        public static bool is_greater_or_equal(string n1, string n2) => are_equal(n1, n2) ^ is_greater(n1, n2);
        public static string switch_sign(string n) => is_negative(n) ? n.Substring(1) : '-' + n;

        public static bool are_equal(string n1, string n2)
        {
            bool n1Negative = is_negative(n1);
            bool n2Negative = is_negative(n2);

            if (n1Negative != n2Negative)
            {
                return false;
            }

            n1 = get_abs(n1);
            n2 = get_abs(n2);
            string[] lst = equivalize(n1, n2);
            n1 = lst[0];
            n2 = lst[1];

            if (n1.Equals(n2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool is_greater(string n, string than)
        {
            string[] lst = equivalize(n, than);
            n = lst[0];
            than = lst[1];

            if (is_positive(n) && is_negative(than))
            {
                return true;
            }
            else if (is_negative(n) && is_positive(than))
            {
                return false;
            }
            else
            {
                n = get_abs(n);
                than = get_abs(than);

                for (int i = 0; i < n.Length; i++)
                {
                    char charN = n[i];
                    char charThan = than[i];

                    if (charN != '.')
                    {
                        if (Char.GetNumericValue(charN) > Char.GetNumericValue(charThan))
                        {
                            return true;
                        }
                        else if (Char.GetNumericValue(charN) < Char.GetNumericValue(charThan))
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }

        static string prepend0s(string n, int amount)
        {
            while (amount != 0)
            {
                amount -= 1;
                n = '0' + n;
            }

            return n;
        }

        static string append0s(string n, int amount)
        {
            while (amount != 0)
            {
                amount -= 1;
                n += '0';
            }

            return n;
        }

        static string[] equivalize(string n1, string n2)
        {
            n1 = to_fraction(n1);
            n2 = to_fraction(n2);
            string n1_int = get_integer_part(n1);
            string n1_fract = get_fraction_part(n1);
            string n2_int = get_integer_part(n2);
            string n2_fract = get_fraction_part(n2);

            if (is_longer(n1_int, n2_int))
            {
                int diff = n1_int.Length - n2_int.Length;
                n2_int = prepend0s(n2_int, diff);
            }
            else
            {
                int diff = n2_int.Length - n1_int.Length;
                n1_int = prepend0s(n1_int, diff);
            }


            if (is_longer(n1_fract, n2_fract))
            {
                int diff = n1_fract.Length - n2_fract.Length;
                n2_fract = append0s(n2_fract, diff);
            }
            else
            {
                int diff = n2_fract.Length - n1_fract.Length;
                n1_fract = append0s(n1_fract, diff);
            }

            string[] n1_n2 = new string[2];
            n1_n2[0] = n1_int + '.' + n1_fract;
            n1_n2[1] = n2_int + '.' + n2_fract;
            return n1_n2;
        }

        static int get_exponent(string n, int index)
        {
            if (is_integer(n))
            {
                return n.Length - index - 1;
            }
            else
            {
                int nullP = n.IndexOf('.');

                if (nullP == index)
                {
                    return -1;
                }
                else if (index > nullP)
                {
                    return (index - nullP) * -1;
                }
                else
                {
                    return nullP - index - 1;
                }
            }
        }

        public static string beautify_number(string n)
        {
            n = to_fraction(n);
            string n_int = get_integer_part(n);
            string n_fract = get_fraction_part(n);
            int trimstart = 0;
            int trimend = 0;

            for (int i = 0; i < n_int.Length; i++)
            {
                if (n_int[i] == '0' && i != n_int.Length - 1)
                {
                    trimstart++;
                }
                else
                {
                    break;
                }
            }

            for (int i = n_fract.Length - 1; i >= 0; i--)
            {
                if (n_fract[i] == '0')
                {
                    if (i != 0)
                    {
                        trimend++;
                    }
                    else
                    {
                        trimend += 2;
                    }
                }
                else
                {
                    break;
                }
            }

            return trim_start(trim_end(n, trimend), trimstart);
        }

        public static string add(List<string> numList)
        {
            string n1 = numList[0];
            numList.RemoveAt(0);

            for (int i = 0; i < numList.Count; i++)
            {
                string n2 = numList[i];

                if (is_positive(n1) && is_positive(n2))
                {
                    n1 = add_positives(n1, n2);
                }
                else if (is_negative(n1) && is_negative(n2))
                {
                    n1 = switch_sign(n1);
                    n2 = switch_sign(n2);
                    n1 = add_positives(n1, n2);
                    n1 = switch_sign(n1);
                }
                else
                {
                    string n_abs = get_abs(n2);
                    string than_abs = get_abs(n1);

                    if (is_greater(n_abs, than_abs))
                    {
                        n1 = subtract(new List<string> { n_abs, than_abs });

                        if (is_negative(n2))
                        {
                            n1 = switch_sign(n1);
                        }
                    }
                    else
                    {
                        n1 = subtract(new List<string> { than_abs, n_abs });

                        if (is_negative(than_abs))
                        {
                            n1 = switch_sign(n1);
                        }
                    }
                }
            }

            return n1;
        }

        public static string subtract(List<string> numList)
        {
            string n1 = numList[0];
            numList.RemoveAt(0);

            for (int i = 0; i < numList.Count; i++)
            {
                string n2 = numList[i];

                if (is_positive(n1) && is_positive(n2))
                {
                    n1 = subtract_positives(n1, n2);
                }
                else if (is_negative(n1) && is_positive(n2))
                {
                    n1 = switch_sign(n1);
                    n2 = switch_sign(n2);
                    n1 = add_positives(n1, n2);
                    n1 = switch_sign(n1);
                }
                else
                {
                    string n_abs = get_abs(n2);
                    string than_abs = get_abs(n1);

                    if (is_greater(n_abs, than_abs))
                    {
                        n1 = subtract(new List<string> { n_abs, than_abs });

                        if (is_negative(n2))
                        {
                            n1 = switch_sign(n1);
                        }
                    }
                    else
                    {
                        n1 = subtract(new List<string> { than_abs, n_abs });

                        if (is_negative(than_abs))
                        {
                            n1 = switch_sign(n1);
                        }
                    }
                }
            }

            return beautify_number(n1);
        }

        public static string multiply(List<string> numList)
        {
            string n1 = numList[0];
            numList.RemoveAt(0);

            for (int i = 0; i < numList.Count; i++)
            {
                string n2 = numList[i];

                if (is_positive(n1) && is_positive(n2))
                {
                    n1 = multiply_positives(n1, n2);
                }
                else if (is_negative(n1) && is_negative(n2))
                {
                    n1 = switch_sign(n1);
                    n2 = switch_sign(n2);
                    n1 = multiply_positives(n1, n2);
                }
                else
                {
                    n1 = get_abs(n1);
                    n2 = get_abs(n2);
                    n1 = multiply_positives(n1, n2);
                    n1 = switch_sign(n1);
                }
            }

            return n1;
        }

        public static string divide(List<string> numList, int accuracy)
        {
            string n1 = numList[0];
            numList.RemoveAt(0);

            for (int i = 0; i < numList.Count; i++)
            {
                string n2 = numList[i];

                if (is_positive(n1) && is_positive(n2))
                {
                    n1 = divide_positives(n1, n2, accuracy);
                }
                else if (is_negative(n1) && is_negative(n2))
                {
                    n1 = switch_sign(n1);
                    n2 = switch_sign(n2);
                    n1 = divide_positives(n1, n2, accuracy);
                }
                else
                {
                    n1 = get_abs(n1);
                    n2 = get_abs(n2);
                    n1 = divide_positives(n1, n2, accuracy);
                    n1 = switch_sign(n1);
                }
            }

            return n1;
        }

        static string add_positives(string n1, string n2)
        {
            string[] lst = equivalize(n1, n2);
            n1 = lst[0];
            n2 = lst[1];
            int pAt = n1.IndexOf('.');
            bool plusOne = false;
            string result = "";

            for (int i = n1.Length - 1; i >= 0; i--)
            {
                char char1 = n1[i];
                char char2 = n2[i];

                if (char1 != '.')
                {
                    double sum = Char.GetNumericValue(char1) + Char.GetNumericValue(char2);

                    if (plusOne)
                    {
                        sum += 1;
                        plusOne = false;
                    }

                    if (sum >= 10)
                    {
                        plusOne = true;
                    }

                    result = sum.ToString()[0] + result;
                }
                else
                {
                    result = '.' + result;
                }
            }

            return beautify_number(result);
        }

        static string subtract_positives(string n1, string n2)
        {
            bool is_negative = false;

            if (is_greater(n2, n1))
            {
                string tmp = n1;
                n1 = n2;
                n2 = tmp;
                is_negative = true;
            }

            string[] lst = equivalize(n1, n2);
            n1 = lst[0];
            n2 = lst[1];
            int pAt = n1.IndexOf('.');
            bool minusOne = false;
            string result = "";

            for (int i = n1.Length - 1; i >= 0; i--)
            {
                char char1 = n1[i];
                char char2 = n2[i];

                if (char1 != '.')
                {
                    double sum;

                    if (i == 0)
                    {
                        sum = Char.GetNumericValue(char1) - Char.GetNumericValue(char2);
                    }
                    else
                    {
                        sum = 10 + Char.GetNumericValue(char1) - Char.GetNumericValue(char2);
                    }

                    if (minusOne)
                    {
                        sum -= 1;
                        minusOne = false;
                    }

                    if (i != 0)
                    {
                        if (sum < 10)
                        {
                            minusOne = true;
                        }
                        else
                        {
                            sum -= 10;
                        }
                    }

                    string tmp = sum.ToString();
                    result = tmp + result;
                }
                else
                {
                    result = '.' + result;
                }
            }

            if (is_negative)
            {
                result = switch_sign(result);
            }

            return beautify_number(result);
        }

        static string multiply_positives(string n1, string n2)
        {
            string product = "0";

            for (int i = 0; i < n1.Length; i++)
            {
                if (n1[i] != '.')
                {
                    double firstVal = Char.GetNumericValue(n1[i]);
                    int firstExp = get_exponent(n1, i);

                    for (int x = 0; x < n2.Length; x++)
                    {
                        if (n2[x] != '.')
                        {
                            double secondVal = Char.GetNumericValue(n2[x]);
                            int secondExp = get_exponent(n2, x);
                            string subProduct = (firstVal * secondVal).ToString();
                            int uniexp = firstExp + secondExp;

                            if (uniexp > 0)
                            {
                                subProduct = append0s(subProduct, uniexp);
                            }
                            else if (uniexp < 0)
                            {
                                if (subProduct.Length == 2)
                                {
                                    if (uniexp == -1)
                                    {
                                        subProduct = subProduct[0] + "." + subProduct[1];
                                    }
                                    else // else if (uniexp < -1)
                                    {
                                        subProduct = "0." + prepend0s(subProduct, (uniexp * -1) - 2);
                                    }
                                }
                                else // elif len(subProduct) == 1:
                                {
                                    subProduct = "0." + prepend0s(subProduct, ((uniexp * -1) - 1));
                                }
                            }

                            product = add(new List<string> { product, subProduct });
                        }
                    }
                }
            }

            return beautify_number(product);
        }

        static string divide_positives(string n1, string n2, int accuracy)
        {
            if (is_longer(get_fraction_part(n1), get_fraction_part(n2)))
            {
                while (get_fraction_part(n1) != "0")
                {
                    n1 = multiply(new List<string> { n1, "10" });
                    n2 = multiply(new List<string> { n2, "10" });
                }
            }
            else
            {
                while (get_fraction_part(n2) != "0")
                {
                    n1 = multiply(new List<string> { n1, "10" });
                    n2 = multiply(new List<string> { n2, "10" });
                }
            }

            int acc = -1;
            string ratio = "0";
            string subRatio = "0";
            int dividieBack = 0;

            while (true)
            {
                if (is_greater(n2, n1))
                {
                    subRatio = get_integer_part(subRatio);

                    if (dividieBack != 0)
                    {
                        subRatio = "0." + prepend0s(subRatio, dividieBack - 1);
                    }

                    ratio = add(new List<string> { ratio, subRatio });
                    subRatio = "0";
                    acc += 1;

                    if (get_integer_part(n1) == "0" || accuracy <= acc)
                    {
                        break;
                    }

                    while (is_greater(n2, n1))
                    {
                        n1 = multiply(new List<string> { n1, "10" });
                        dividieBack += 1;
                    }
                }

                n1 = subtract(new List<string> { n1, n2 });
                subRatio = add(new List<string> { subRatio, "1" });
            }

            return beautify_number(ratio);
        }

        public static string remainder(string divident, string divisor)
        {
            string ratio = divide(new List<string> { divident, divisor }, 0);
            string remainder = subtract(new List<string> { divident, multiply(new List<string> { ratio, divisor }) });
            return beautify_number(remainder);
        }

        public static string exponentiate(string number, string exponent, int precision)
        {
            string originalNumber = number;

            if (are_equal(exponent, "0"))
            {
                number = "1";
            }
            else if (!are_equal(exponent, "1"))
            {
                bool exponentIsNegative = false;

                if (is_negative(exponent))
                {
                    exponentIsNegative = true;
                    exponent = get_abs(exponent);
                }

                for (int i = 1; is_greater(exponent, i.ToString()); i++)
                {
                    number = multiply(new List<string> { number, originalNumber });
                }

                if (exponentIsNegative)
                {
                    number = divide(new List<string> { "1", number }, precision);
                }
            }

            return beautify_number(number);
        }
    }
}