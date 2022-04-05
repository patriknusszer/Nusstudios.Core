using System;
using System.Collections.Generic;
using Nusstudios.Core.UnmanagedTypes;
using System.Numerics;

namespace Nusstudios.Core.Parsing.Unicode
{
    public enum Encoding
    {
        UTF8,
        UTF16LE,
        UTF16BE,
        UTF32LE,
        UTF32BE
    }

    public class UnicodeConsts
    {
        public const UInt64 FormFeed = 0xC;
        public const UInt64 BackSpace = 0x8;

        public const UInt64 Space = 0x20;
        public const UInt64 CarriageReturn = 0xD;
        public const UInt64 LineFeed = 0xA;
        public const UInt64 Tab = 0x9;

        public const UInt64 BackSlash = 0x5C;
        public const UInt64 ForwardSlash = 0x2F;
        public const UInt64 Quote = 0x22;
        public const UInt64 Colon = 0x3A;
        public const UInt64 Comma = 0x2C;
        public const UInt64 Dot = 0x2E;
        public const UInt64 SquareBracketOpen = 0x5B;
        public const UInt64 SquareBracketClose = 0x5D;
        public const UInt64 CurlyBracketOpen = 0x7B;
        public const UInt64 CurlyBracketClose = 0x7D;

        public const UInt64 Plus = 0x2B;
        public const UInt64 Minus = 0x2D;

        public const UInt64 Zero = 0x30;
        public const UInt64 One = 0x31;
        public const UInt64 Two = 0x32;
        public const UInt64 Three = 0x33;
        public const UInt64 Four = 0x34;
        public const UInt64 Five = 0x35;
        public const UInt64 Six = 0x36;
        public const UInt64 Seven = 0x37;
        public const UInt64 Eight = 0x38;
        public const UInt64 Nine = 0x39;

        public const UInt64 a = 0x61;
        public const UInt64 b = 0x62;
        public const UInt64 c = 0x63;
        public const UInt64 d = 0x64;
        public const UInt64 e = 0x65;
        public const UInt64 f = 0x66;
        public const UInt64 g = 0x67;
        public const UInt64 h = 0x68;
        public const UInt64 i = 0x69;
        public const UInt64 j = 0x6A;
        public const UInt64 k = 0x6B;
        public const UInt64 l = 0x6C;
        public const UInt64 m = 0x6D;
        public const UInt64 n = 0x6E;
        public const UInt64 o = 0x6F;
        public const UInt64 p = 0x70;
        public const UInt64 q = 0x71;
        public const UInt64 r = 0x72;
        public const UInt64 s = 0x73;
        public const UInt64 t = 0x74;
        public const UInt64 u = 0x75;
        public const UInt64 v = 0x76;
        public const UInt64 w = 0x77;
        public const UInt64 x = 0x78;
        public const UInt64 y = 0x79;
        public const UInt64 z = 0x7A;

        public const UInt64 A = 0x41;
        public const UInt64 B = 0x42;
        public const UInt64 C = 0x43;
        public const UInt64 D = 0x44;
        public const UInt64 E = 0x45;
        public const UInt64 F = 0x46;
        public const UInt64 G = 0x47;
        public const UInt64 H = 0x48;
        public const UInt64 I = 0x49;
        public const UInt64 J = 0x4A;
        public const UInt64 K = 0x4B;
        public const UInt64 L = 0x4C;
        public const UInt64 M = 0x4D;
        public const UInt64 N = 0x4E;
        public const UInt64 O = 0x4F;
        public const UInt64 P = 0x50;
        public const UInt64 Q = 0x51;
        public const UInt64 R = 0x52;
        public const UInt64 S = 0x53;
        public const UInt64 T = 0x54;
        public const UInt64 U = 0x55;
        public const UInt64 V = 0x56;
        public const UInt64 W = 0x57;
        public const UInt64 X = 0x58;
        public const UInt64 Y = 0x59;
        public const UInt64 Z = 0x5A;
    }

    public static class UnicodeCore
    {
        public static BigRational InterpretAsMantissa(UInt64[] codepoints)
        {
            if (!IsNumber(codepoints))
                throw new Exception();

            BigRational d = 0;

            for (int i = 1; i <= codepoints.Length; i++)
                d = d + (BigRational.Pow(10, i * -1) * ReadDigit(codepoints[i - 1]));

            return d;
        }

        public static BigRational InterpretAsMantissa(UnicodeStream stream) => InterpretAsMantissa(stream.CodePoints.ToArray());

        public static BigRational FractionToDouble(UnicodeStream stream, UInt64 decPt)
        {
            UInt64[][] fract = stream.Split(decPt, true);

            if (fract.Length != 2)
                throw new Exception();

            if (!IsNumber(fract[0]) | !IsNumber(fract[1]))
                throw new Exception();


            BigRational d = 0;

            for (int i = 0; i < fract[0].Length; i++)
                d = d + (BigRational.Pow(10, i) * ReadDigit(fract[0][fract[0].Length - (i + 1)]));

            for (int i = 1; i <= fract[1].Length; i++)
                d = d + (BigRational.Pow(10, i * -1) * ReadDigit(fract[1][i - 1]));

            return d;
        }

        public static UInt64 InterpretAsInteger(UInt64[] codepoints)
        {
            if (!IsNumber(codepoints))
                throw new Exception();

            UInt64 n = 0;

            for (int i = 0; i < codepoints.Length; i++)
                n += (UInt64)Math.Pow(10, i) * (UInt64)ReadDigit(codepoints[codepoints.Length - (i + 1)]);

            return n;
        }

        public static UInt64 InterpretAsInteger(UnicodeStream stream)
        {
            UInt64 dec = 0;

            for (int i = 0; i < stream.Length; i++)
                dec += (UInt64)Math.Pow(10, i) * (UInt64)ReadDigit(stream.CharAt(stream.Length - (i + 1)));

            return dec;
        }

        public static UInt64 Hex2Decimal(UnicodeStream stream)
        {
            UInt64 dec = 0;

            for (int i = 0; i < stream.Length; i++)
                dec += (UInt64)Math.Pow(16, i) * (UInt64)ReadHexValue(stream.CharAt(stream.Length - (i + 1)));

            return dec;
        }

        public static int ReadDigit(UInt64 codepoint)
        {
            if (IsDigit(codepoint))
                return (int)(codepoint - UnicodeConsts.Zero);
            else
                throw new Exception();
        }

        public static int ReadHexValue(UInt64 codepoint)
        {
            if (IsLetter(codepoint))
            {
                if (IsUpperCaseLetter(codepoint))
                    return (int)(10 + (codepoint - UnicodeConsts.A));
                else
                    return (int)(10 + (codepoint - UnicodeConsts.a));
            }
            else if (IsDigit(codepoint))
                return (int)(codepoint - UnicodeConsts.Zero);
            else
                throw new Exception();
        }

        public static bool IsLetter(UInt64 codepoint)
        {
            if (IsUpperCaseLetter(codepoint) ^ IsLowerCaseLetter(codepoint))
                return true;
            else
                return false;
        }

        public static bool IsUpperCaseLetter(UInt64 codepoint)
        {
            if (codepoint >= UnicodeConsts.A && codepoint <= UnicodeConsts.Z)
                return true;
            else
                return false;
        }

        public static bool IsLowerCaseLetter(UInt64 codepoint)
        {
            if (codepoint >= UnicodeConsts.a && codepoint <= UnicodeConsts.z)
                return true;
            else
                return false;
        }

        public static bool IsDigit(UInt64 codepoint)
        {
            if (codepoint >= UnicodeConsts.Zero && codepoint <= UnicodeConsts.Nine)
                return true;
            else
                return false;
        }

        public static bool IsNumber(UnicodeStream stream)
        {
            for (int i = 0; i < stream.Length; i++)
                if (!IsDigit(stream.CharAt(i)))
                    return false;

            return true;
        }

        public static bool IsNumber(UInt64[] stream)
        {
            for (int i = 0; i < stream.Length; i++)
                if (!IsDigit(stream[i]))
                    return false;

            return true;
        }

        private static int getUTF8len(byte b)
        {
            int exp = 7;
            int num = 0;

            for (int e = exp; e > 0; e--)
            {
                if (b >= Math.Pow(2, e))
                {
                    b -= (byte)Math.Pow(2, e);
                    num++;
                }
                else
                    break;
            }

            if (num == 1)
                throw new Exception();
            else if (num == 0)
                return 1;
            else
                return num;
        }

        internal static byte[] parseCodePointsToUTFStream(List<UInt64> codePoints, Encoding enc)
        {
            List<byte> stream = new List<byte>();

            for (int i = 0; i < codePoints.Count; i++)
                stream.AddRange(EncodeCP(codePoints[i], enc));

            return stream.ToArray();
        }

        internal static List<UInt64> parseUTFStreamToCodePoints(byte[] buffer, Encoding enc)
        {
            int i = 0;
            List<UInt64> codepoints = new List<ulong>();

            while (i != buffer.Length)
                codepoints.Add(DecodeCPAt(buffer, ref i, enc));

            return codepoints;
        }

        public static byte[] EncodeCP(UInt64 codepoint, Encoding enc)
        {
            if (Encoding.UTF8 == enc)
            {
                /* if (codepoint > 0x1000000000)
                    throw new Exception();
                else
                {
                    int numlen = 0;
                    ulong temp = codepoint;

                    while (temp >= Math.Pow(2, numlen))
                        temp -= (ulong)Math.Pow(2, numlen++);

                    numlen++;

                    if (numlen <= 7)
                    {
                        return new byte[1] { (byte)codepoint };
                    }
                    else
                    {
                        int numCodeUnits;

                        for (numCodeUnits = 2; numCodeUnits < 8; numCodeUnits++)
                        {
                            int space = 8 - (numCodeUnits + 1);
                            space += (numCodeUnits - 1) * 6;
                            if (numlen <= space)
                                break;
                        }

                        byte[] encoded = new byte[numCodeUnits];

                        for (int i = 1; i <= numCodeUnits - 1; i++)
                        {
                            byte sixPack = (byte)(((byte)(codepoint >> 6 * (i - 1))) & 0x3F);
                            byte codeUnit = sixPack |= 0x80;
                            encoded[encoded.Length - i] = codeUnit;
                        }

                        byte firstCodeUnit = 0;
                        firstCodeUnit |= (byte)(codepoint >> 6 * (numCodeUnits - 1));

                        for (int exp = 7, len = 0; len < numCodeUnits; len++, exp--)
                        {
                            firstCodeUnit += (byte)Math.Pow(2, exp);
                        }

                        encoded[0] = firstCodeUnit;
                        return encoded;
                    }
                } */

                if (codepoint == 0) return new byte[] { 0 };
                int e;
                UInt64 cpy = codepoint;

                for (e = 0; codepoint > Math.Pow(2, e); e++)
                    cpy -= (ulong)Math.Pow(2, e);

                e++;
                int codeunits = 1;
                int continuations = 0;

                if (e <= 7)
                {
                    codeunits = 1;
                    continuations = 0;
                    return new byte[] { (byte)codepoint };
                }
                else
                {
                    int space = 0;

                    for (codeunits = 1; e > space;)
                    {
                        codeunits++;
                        space = 0;
                        int lenBits = codeunits + 1;
                        int total = codeunits * 8;
                        total -= lenBits;
                        int spaceOnLength = total % 8;
                        space = spaceOnLength;
                        continuations = total / 8;
                        space += continuations * 6;
                    }

                    cpy = codepoint;
                    byte[] encoded = new byte[codeunits];

                    for (int i = 1; i <= continuations; i++)
                    {
                        byte cu = (byte)(((byte)cpy) & 0x3F);
                        cu |= 128;
                        encoded[encoded.Length - i] |= cu;
                        cpy >>= 6;
                    }

                    if (cpy != 0)
                        encoded[encoded.Length - (continuations + 1)] = (byte)cpy;

                    int lenFullBytes = codeunits / 8;
                    int lenRemBits = codeunits % 8;

                    for (int i = 0; i < lenFullBytes; i++)
                        encoded[i] = 255;

                    byte mask = 0;

                    for (int i = 7; i > (7 - lenRemBits); i--)
                        mask += (byte)Math.Pow(2, i);

                    encoded[lenFullBytes] |= mask;
                    return encoded;
                }
            }
            else if (Encoding.UTF16BE == enc ^ Encoding.UTF16LE == enc)
            {
                if (codepoint < 0x10000)
                    return ByteConverter.UInt16ToBytes((ushort)codepoint, Encoding.UTF16LE == enc);
                else
                {
                    ulong cp = codepoint - 0x10000;
                    UInt16 hs = 0xD800;
                    UInt16 ls = 0xDC00;
                    hs |= (ushort)(cp >> 10);
                    ls |= (ushort)(cp & 0x3FF);
                    byte[] hs_bytes = ByteConverter.UInt16ToBytes(hs, Encoding.UTF16LE == enc);
                    byte[] ls_bytes = ByteConverter.UInt16ToBytes(ls, Encoding.UTF16LE == enc);
                    byte[] encoded = new byte[4];
                    Array.Copy(hs_bytes, 0, encoded, 0, 2);
                    Array.Copy(ls_bytes, 0, encoded, 2, 2);
                    return encoded;
                }
            }
            else
                return ByteConverter.UInt32ToBytes((uint)codepoint, Encoding.UTF32LE == enc);
        }

        public static UInt64 DecodeCPAt(byte[] buff, ref int position, Encoding enc)
        {
            if (Encoding.UTF8 == enc)
            {
                if (buff[0] < 128)
                    return buff[0];
                if ((buff[0] & 0xC0) == 128)
                    throw new Exception();

                int codeunits = 0;
                int i;

                for (i = 0; buff[i] == 255; i++)
                    codeunits += 8;

                byte cpy = buff[i];
                int x;

                for (x = 7; cpy >= (byte)Math.Pow(2, x); x--, codeunits++)
                    cpy -= (byte)Math.Pow(2, x);

                int shift = 8 - (x + 1);
                cpy = (byte)(buff[i] << shift);
                cpy >>= shift;
                int continuations = codeunits - (i + 1);
                ulong res = cpy;

                for (int y = ++i; y < i + continuations; y++)
                {
                    res <<= 6;
                    byte cu = buff[y];

                    if ((cu & 0xC0) != 128)
                        throw new Exception();

                    cu &= 0x3F;
                    res |= cu;
                }

                return res;

                /* byte first = buff[position++];
                int len = getUTF8len(first);
                UInt64 mask;

                if (len != 1)
                    mask = ((UInt64)(((byte)(first << (len + 1))) >> (len + 1))) << ((len - 1) * 6);
                else
                    mask = first;

                UInt64 codepoint = 0;
                codepoint |= mask;

                for (int i = position; i < len; i++)
                {
                    if ((buff[i] & 0xC0) != 0x80)
                    {
                        throw new Exception();
                    }

                    byte b = (byte)(buff[i] & 0x3f);
                    mask = ((UInt64)b) << ((len - (i + 1)) * 6);
                    codepoint |= mask;
                }

                return codepoint; */
            }
            else if (Encoding.UTF16BE == enc ^ Encoding.UTF16LE == enc)
            {
                if ((buff.Length - position) % 2 != 0)
                    throw new Exception();

                byte[] sub = new byte[2];
                Array.Copy(buff, position, sub, 0, 2);
                position += 2;

                if (Encoding.UTF16BE == enc)
                    Array.Reverse(sub);

                UInt16 first = (ushort)((sub[1] << 8) | sub[0]);

                if (first < 0xD800 ^ first > 0xDFFF)
                    return first;
                else if (first >= 0xD800 && first <= 0xDBFF)
                {
                    if ((buff.Length - position) < 2)
                        throw new Exception();
                    else
                    {
                        Array.Copy(buff, position, sub, 0, 2);
                        position += 2;

                        if (Encoding.UTF16BE == enc)
                            Array.Reverse(sub);

                        UInt16 last = (ushort)((sub[1] << 8) | sub[0]);

                        if (last >= 0xDC00 && last <= 0xDFFF)
                        {
                            first = (ushort)(first & 0x3FF);
                            last = (ushort)(last & 0x3FF);
                            return ((UInt32)first << 10) | ((UInt32)last) + 0x10000;
                        }
                        else
                            throw new Exception();
                    }
                }
                else
                    throw new Exception();
            }
            else
            {
                if ((buff.Length - position) % 4 != 0)
                    throw new Exception();

                byte[] sub = new byte[4];
                Array.Copy(buff, position, sub, 0, 4);
                position += 4;

                if (Encoding.UTF32BE == enc)
                    Array.Reverse(sub);

                UInt32 codepoint = 0;

                for (int i = 0; i < 4; i++)
                {
                    UInt32 mask = (uint)(sub[i] << (i * 8));
                    codepoint |= mask;
                }

                return codepoint;
            }
        }
    }
}
