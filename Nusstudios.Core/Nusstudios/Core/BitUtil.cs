using System;
using System.Collections;
using System.Collections.Generic;

namespace Nusstudios.Core {
    class BitUtil
    {
        public enum GetBitsInEx_MODE
        {
            EXCLUDE,
            INCLUDE
        }

        public enum BITS
        {
            FIRST = 1,
            SECOND = 2,
            THIRD = 4,
            FOURTH = 8,
            FIFTH = 16,
            SIXTH = 32,
            SEVENTH = 64,
            EIGHTH = 128
        }

        public enum ANDBITGETTER
        {
            // --------- SEQUENCES ---------

            // 0111 1111 -> lower 7 bits of a byte
            LOW7 = 127,
            LOW6 = 63,
            LOW5 = 31,
            LOW4 = 15,
            LOW3 = 7,
            LOW2 = 3,
            HIGH2 = 192,
            HIGH3 = 224,
            HIGH4 = 240,
            HIGH5 = 248,
            HIGH6 = 252,
            HIGH7 = 254,

            // --------- SINGLES ---------

            FIRST = 1,
            SECOND = 2,
            THIRD = 4,
            FOURTH = 8,
            FIFTH = 16,
            SIXTH = 32,
            SEVENTH = 64,
            EIGHTH = 128,

            // --------- GAPPED SEQUENCES ---------

            EXCEPTSEVENTH = 255 - SEVENTH,
            EXCEPTSIXTH = 255 - SIXTH,
            EXCEPTFIFTH = 255 - FIFTH,
            EXCEPTFOURTH = 255 - FOURTH,
            EXCEPTTHIRD = 255 - THIRD,
            EXCEPTSECOND = 255 - SECOND

            // For other sequences there are in and exclusions + ranges
        }

        public static byte ReverseBits(byte b)
        {
            BitArray original = new BitArray(b);
            BitArray _new = new BitArray(new byte());

            for (int i = 0, x = 7; i < 8; i++, x--)
            {
                _new.Set(i, original.Get(x));
            }

            byte[] _b = new byte[1];
            _new.CopyTo(_b, 0);
            return _b[0];
        }

        // variable "ubyte" should range from 0 to 255
        public static byte UByteToByte(short ubyte)
        {
            if (ubyte > 127)
            {
                return (byte)((128 - (ubyte - 127 - 1)) * -1);
            }
            else
            {
                return (byte)ubyte;
            }
        }

        // the return value ranges from 0 to 255
        public static short ByteToUByte(byte b)
        {
            if (b < 0)
            {
                return (short)(128 + (128 - (b * -1)));
            }
            else
            {
                return b;
            }
        }

        public static byte[] ReverseBuffer(byte[] buffer)
        {
            byte[] reversed = new byte[buffer.Length];

            for (int i = buffer.Length - 1, x = 0; i >= 0; i--, x++)
            {
                reversed[x] = buffer[i];
            }

            return reversed;
        }

        // for use with BITS
        public static byte SetBit(byte b, int nthBit, bool bit)
        {
            byte ret;

            if (bit)
            {
                ret = (byte)(b + nthBit);
            }
            else
            {
                ret = (byte)(b - nthBit);
            }

            return ret;
        }

        // for use with ANDBITGETTER
        public static byte GetBits(byte b, int bits)
        {
            return (byte)(b & bits);
        }

        // for use with ANDBITGETTER
        public static byte GetBits(byte b, List<ANDBITGETTER> andbitgetters, GetBitsInEx_MODE mode)
        {
            int num = b;

            foreach (int range in andbitgetters)
            {
                num -= (mode == GetBitsInEx_MODE.EXCLUDE ? num : num * -1);
            }

            return (byte)num;
        }

        public static byte GetBits(byte b, int littleendianbitpos, int bitnum, bool advanceTowardsLSB)
        {
            if (GetOverflow(littleendianbitpos, bitnum, advanceTowardsLSB) == 0) {
                if (advanceTowardsLSB) {
                    b <<= (8 - (littleendianbitpos + 1));
                    b >>= (8 - (littleendianbitpos + 1));
                    b >>= (8 - ((8 - (littleendianbitpos + 1)) + bitnum));
                }
                else {
                    b <<= (8 - (littleendianbitpos + bitnum));
                    b >>= (8 - (littleendianbitpos + bitnum));
                    b >>= littleendianbitpos;
                }

                return b;
            }
            else {
                throw new Exception();
            }
        }

        // for use with BITS
        public static bool GetBit(byte b, BITS nthBit)
        {
            return (b & (int)nthBit) != 0;
        }

        public static void Read(byte[] from, ref int fromPos, ref byte[] to, int toPos, int amount)
        {
            Array.Copy(from, fromPos, to, toPos, amount);
            fromPos += amount;
        }

            public static void ReadByte(byte[] from, ref int fromPos, out byte to)
            {
                to = from[fromPos];
                fromPos++;
            }

            private static string BitAlign(string bitsequence, int to)
        {
            if (bitsequence.Length < to)
            {
                bitsequence = StringUtil.PrependChar(bitsequence, "0", to - bitsequence.Length);
            }

            return bitsequence;
        }

        public static string byteToString(byte b)
        {
            int bytepos = 0;
            int bitpos = 7;
            return ReadBits(new byte[] { b }, ref bytepos, ref bitpos, 8, true);
        }

        public static byte stringToByte(string str)
        {
            return (byte)Convert.ToInt32(BaseConverter.Convert(str, 2, 10, 0, 0));
        }

        public static byte[] StringArrToByteArr(string[] strArr)
        {
            byte[] array = new byte[strArr.Length];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = stringToByte(strArr[i]);
            }

            return array;
        }

        public static string[] ByteArrToStringArr(byte[] byteArr)
        {
            string[] array = new string[byteArr.Length];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = byteToString(byteArr[i]);
            }

            return array;
        }

        public static int BitnumFromIndex(int littleendianbitpos, bool advanceTowardsLSB)
        {
            if (advanceTowardsLSB)
            {
                return littleendianbitpos + 1;
            }
            else
            {
                return 8 - littleendianbitpos;
            }
        }

        public static int GetOverflow(int littleendianbitpos, int bitnum, bool advanceTowardsLSB)
        {
            int overflow = 0;

            if (advanceTowardsLSB)
            {
                if (littleendianbitpos - bitnum < -1)
                {
                    overflow = bitnum - (littleendianbitpos + 1);
                }
            }
            else
            {
                if (littleendianbitpos + bitnum > 8)
                {
                    overflow = bitnum - (8 - littleendianbitpos);
                }
            }

            return overflow;
        }

        public static byte WriteToByte(byte[] buffer, ref int bytepos, ref int littleendianbitpos, string bitsequence, bool advanceTowardsLSB)
        {
            if (GetOverflow(littleendianbitpos, bitsequence.Length, advanceTowardsLSB) == 0)
            {
                byte b = buffer[bytepos];

                for (int i = 0; i < bitsequence.Length; i++)
                {
                    b = SetBit(b, (int)Math.Pow(2, littleendianbitpos), bitsequence[i] == '1');
                    littleendianbitpos += (advanceTowardsLSB ? -1 : 1);
                }

                if (littleendianbitpos == 8)
                {
                    bytepos++;
                    littleendianbitpos = 0;
                }
                else if (littleendianbitpos == -1)
                {
                    bytepos++;
                    littleendianbitpos = 7;
                }

                return b;
            }
            else
            {
                throw new Exception();
            }
        }

        public static byte[] WriteBits(byte[] buffer, ref int bytepos, ref int littleendianbitpos, string bitsequence, bool advanceTowardsLSB)
        {
            int overflow = GetOverflow(littleendianbitpos, bitsequence.Length, advanceTowardsLSB);
            int endbyte = bytepos + (overflow / 8) + (overflow % 8 == 0 ? 0 : 1);

            if (endbyte <= buffer.Length - 1)
            {
                if (overflow == 0)
                {
                    buffer[bytepos] = WriteToByte(buffer, ref bytepos, ref littleendianbitpos, bitsequence, advanceTowardsLSB);
                }
                else
                {
                    int writeBitsToFirstByte = BitnumFromIndex(littleendianbitpos, advanceTowardsLSB);
                    int remainder;
                    int numWholeBytes;

                    if (writeBitsToFirstByte != 8)
                    {
                        string subsequence = bitsequence.Substring(0, writeBitsToFirstByte);
                        buffer[bytepos] = WriteToByte(buffer, ref bytepos, ref littleendianbitpos, subsequence, advanceTowardsLSB);
                        remainder = (bitsequence.Length - writeBitsToFirstByte) % 8;
                        numWholeBytes = (bitsequence.Length - writeBitsToFirstByte) / 8;
                    }
                    else
                    {
                        remainder = bitsequence.Length % 8;
                        numWholeBytes = bitsequence.Length / 8;
                    }

                    if (numWholeBytes != 0)
                    {
                        string wholeBytes = bitsequence.Substring(writeBitsToFirstByte == 8 ? 0 : writeBitsToFirstByte, bitsequence.Length - remainder);
                        string[] binbytes = StringUtil.SplitInParts(wholeBytes, 8);

                        for (int i = 0; i < binbytes.Length; i++)
                        {
                            buffer[bytepos] = WriteToByte(buffer, ref bytepos, ref littleendianbitpos, binbytes[i], advanceTowardsLSB);
                        }
                    }

                    if (remainder != 0)
                    {
                        string _remainder = bitsequence.Substring(bitsequence.Length - remainder);
                        buffer[bytepos] = WriteToByte(buffer,ref bytepos, ref littleendianbitpos, _remainder, advanceTowardsLSB);
                    }
                }

                return buffer;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string ReadFromByte(byte[] buffer, ref int bytepos, ref int littleendianbitpos, int bitnum, bool advanceTowardsLSB)
        {
            int overflow = GetOverflow(littleendianbitpos, bitnum, advanceTowardsLSB);

            if (bytepos <= buffer.Length - 1 && overflow == 0)
            {
                byte b = buffer[bytepos];

                if (advanceTowardsLSB)
                {
                    b = (byte)(b << (8 - (littleendianbitpos + 1)));
                    b >>= (8 - (littleendianbitpos + 1));
                    b >>= (8 - (8 - (littleendianbitpos + 1) + bitnum));
                    littleendianbitpos = littleendianbitpos - bitnum;

                    if (littleendianbitpos == -1)
                    {
                        littleendianbitpos = 7;
                        bytepos++;
                    }
                }
                else
                {
                    b <<= (8 - (littleendianbitpos + bitnum));
                    b >>= (8 - (littleendianbitpos + bitnum));
                    b >>= littleendianbitpos;
                    littleendianbitpos %= 8;
                    bytepos++;
                }


                string binary = "";
                binary = BaseConverter.Convert(b.ToString(), 10, 2, 0, 0);

                // leading zeros are left out, must align bits to byte size
                binary = BitAlign(binary, bitnum);
                return binary;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string ReadBits(byte[] buffer, ref int bytepos, ref int littleendianbitpos, int bitnum, bool advanceTowardsLSB)
        {
            string part;
            string binary = "";
            int overflow = GetOverflow(littleendianbitpos, bitnum, advanceTowardsLSB);
            int endbyte = bytepos + (overflow / 8) + (overflow % 8 == 0 ? 0 : 1);

            if (endbyte <= buffer.Length - 1)
            {
                if (overflow == 0)
                {
                    return ReadFromByte(buffer, ref bytepos, ref littleendianbitpos, bitnum, advanceTowardsLSB);
                }
                else
                {
                    int numWholeBytes;
                    int requiredBitsFromFirstByte = BitnumFromIndex(littleendianbitpos, advanceTowardsLSB);
                    int remainder;

                    if (requiredBitsFromFirstByte != 8)
                    {
                        binary = ReadFromByte(buffer, ref bytepos, ref littleendianbitpos, requiredBitsFromFirstByte, advanceTowardsLSB);
                        numWholeBytes = (bitnum - requiredBitsFromFirstByte) / 8;
                        remainder = (bitnum - requiredBitsFromFirstByte) % 8;
                    }
                    else
                    {
                        numWholeBytes = bitnum / 8;
                        remainder = bitnum % 8;
                    }

                    // bit sequence goes through whole bytes too, but not neccessarily
                    for (int i = 0; i < numWholeBytes; i++)
                    {
                        part = ReadFromByte(buffer, ref bytepos, ref littleendianbitpos, 8, advanceTowardsLSB);
                        binary += part;
                    }

                    if (remainder != 0)
                    {
                        part = ReadFromByte(buffer, ref bytepos, ref littleendianbitpos, 8, advanceTowardsLSB);
                        binary += part;
                    }

                    return binary;
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}