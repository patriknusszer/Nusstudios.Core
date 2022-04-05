using System;
using System.Numerics;
using System.Collections.Generic;

namespace Nusstudios.Core {
    public class ByteConverter
    {
        public static byte[] Int16ToBytes(short number, bool littleEndian)
        {
            byte[] buffer = new byte[2];

            for (int i = 1; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] Int16ToBytesCalc(short number, bool littleEndian)
        {
            bool IsNegative = false;

            if (number < 0)
            {
                /*
                alternative code:
                number += 1;
                number * -1;
                 */
                number = (short)~number;
                IsNegative = true;
            }

            byte[] buffer = new byte[2];
            short timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 2; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = (short)(timesForNextDigit / 256);
                buffer[i] = remainderForCurrentDigit;

                if (IsNegative)
                {
                    buffer[i] = (byte)~buffer[i];
                }
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] Int32ToBytes(int number, bool littleEndian)
        {
            byte[] buffer = new byte[4];

            for (int i = 3; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] Int32ToBytesCalc(int number, bool littleEndian)
        {
            bool isNegative = false;

            if (number < 0)
            {
                /*
                alternative code:
                number += 1;
                number * -1;
                 */
                number = ~number;
                isNegative = true;
            }

            byte[] buffer = new byte[4];
            int timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 4; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = timesForNextDigit / 256;
                buffer[i] = remainderForCurrentDigit;

                if (isNegative)
                {
                    buffer[i] = (byte)~buffer[i];
                }
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] Int64ToBytes(long number, bool littleEndian)
        {
            byte[] buffer = new byte[8];

            for (int i = 7; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] Int64ToBytesCalc(long number, bool littleEndian)
        {
            bool isNegative = false;

            if (number < 0)
            {
                /*
                alternative code:
                number += 1;
                number * -1;
                 */
                number = ~number;
                isNegative = true;
            }

            byte[] buffer = new byte[8];
            long timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 8; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = timesForNextDigit / 256;
                buffer[i] = remainderForCurrentDigit;

                if (isNegative)
                {
                    buffer[i] = (byte)~buffer[i];
                }
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt16ToBytes(ushort number, bool littleEndian)
        {
            byte[] buffer = new byte[2];

            for (int i = 1; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt16ToBytesCalc(ushort number, bool littleEndian)
        {
            byte[] buffer = new byte[2];
            ushort timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 2; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = (ushort)(timesForNextDigit / 256);
                buffer[i] = remainderForCurrentDigit;
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt32ToBytes(uint number, bool littleEndian)
        {
            byte[] buffer = new byte[4];

            for (int i = 3; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt32ToBytesCalc(uint number, bool littleEndian)
        {
            byte[] buffer = new byte[4];
            uint timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 4; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = timesForNextDigit / 256;
                buffer[i] = remainderForCurrentDigit;
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt64ToBytes(ulong number, bool littleEndian)
        {
            byte[] buffer = new byte[2];

            for (int i = 7; i >= 0; i--)
            {
                buffer[i] = (byte)(number >> (i * 8));
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UInt64ToBytesCalc(ulong number, bool littleEndian)
        {
            byte[] buffer = new byte[8];
            ulong timesForNextDigit = number;
            byte remainderForCurrentDigit;

            for (int i = 0; i < 8; i++)
            {
                remainderForCurrentDigit = (byte)(timesForNextDigit % 256);
                timesForNextDigit = timesForNextDigit / 256;
                buffer[i] = remainderForCurrentDigit;
            }

            if (!littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static short BytesToInt16(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[2];
            Array.Copy(buffer, offset, subBuffer, 0, 2);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            short number = 0;

            for (int i = 1; i >= 0; i--)
            {
                short tmp = (short)(subBuffer[i] << (i * 8));
                number |= tmp;
            }

            return number;
        }

        public static short BytesToInt16Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[2];
            Array.Copy(buffer, offset, subBuffer, 0, 2);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            short number = 0;
            bool isNegative = false;

            if (BitUtil.ByteToUByte(subBuffer[1]) >= 128)
            {
                isNegative = true;
            }

            for (int i = 0; i < 2; i++)
            {
                if (isNegative)
                {
                    subBuffer[i] = (byte)~subBuffer[i];
                }

                number += (short)(subBuffer[i] * (short)Math.Pow(256, i));
            }

            if (isNegative)
            {
                /*
                alternative code:
                number *= -1;
                number -= 1;
                 */
                number = (short)~number;
            }

            return number;
        }

        public static int BytesToInt32(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[4];
            Array.Copy(buffer, offset, subBuffer, 0, 4);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            int number = 0;

            for (int i = 3; i >= 0; i--)
            {
                int tmp = subBuffer[i] << (i * 8);
                number |= tmp;
            }

            return number;
        }

        public static int BytesToInt32Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[4];
            Array.Copy(buffer, offset, subBuffer, 0, 4);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            int number = 0;
            bool isNegative = false;

            if (BitUtil.ByteToUByte(subBuffer[3]) >= 128)
            {
                isNegative = true;
            }

            for (int i = 0; i < 4; i++)
            {
                if (isNegative)
                {
                    subBuffer[i] = (byte)~subBuffer[i];
                }

                number += subBuffer[i] * (int)Math.Pow(256, i);
            }

            if (isNegative)
            {
                /*
                alternative code:
                number *= -1;
                number -= 1;
                 */
                number = ~number;
            }

            return number;
        }

        public static long BytesToInt64(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            long number = 0;

            for (int i = 7; i >= 0; i--)
            {
                long tmp = (((long)subBuffer[i]) << (i * 8));
                number |= tmp;
            }

            return number;
        }

        public static long BytesToInt64Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            long number = 0;
            bool isNegative = false;

            if (BitUtil.ByteToUByte(subBuffer[7]) >= 128)
            {
                isNegative = true;
            }

            for (int i = 0; i < 8; i++)
            {
                if (isNegative)
                {
                    subBuffer[i] = (byte)~subBuffer[i];
                }

                number += (subBuffer[i] * (long)BigInteger.Pow(256, i));
            }

            if (isNegative)
            {
                /*
                alternative code:
                number *= -1;
                number -= 1;
                 */
                number = ~number;
            }

            return number;
        }

        public static ushort BytesToUInt16(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[2];
            Array.Copy(buffer, offset, subBuffer, 0, 2);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            ushort number = 0;

            for (int i = 1; i >= 0; i--)
            {
                ushort tmp = (ushort)(subBuffer[i] << (i * 8));
                number |= tmp;
            }

            return number;
        }

        public static ushort BytesToUInt16Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[2];
            Array.Copy(buffer, offset, subBuffer, 0, 2);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            ushort number = 0;

            for (int i = 0; i < 2; i++)
            {
                // ushort casting, if was possible, would be ok as well
                number += (ushort)(subBuffer[i] * (ushort)Math.Pow(256, i));
            }

            return number;
        }

        public static uint BytesToUInt32(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[4];
            Array.Copy(buffer, offset, subBuffer, 0, 4);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            uint number = 0;

            for (int i = 3; i >= 0; i--)
            {
                uint tmp = (uint)(subBuffer[i] << (i * 8));
                number |= tmp;
            }

            return number;
        }

        public static uint BytesToUInt32Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[4];
            Array.Copy(buffer, offset, subBuffer, 0, 4);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            uint number = 0;

            for (int i = 0; i < 4; i++)
            {
                // uint casting, if was possible, would be ok as well
                number += subBuffer[i] * (uint)Math.Pow(256, i);
            }

            return number;
        }

        public static ulong BytesToUInt64(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            ulong number = 0;

            for (int i = 7; i >= 0; i--)
            {
                ulong tmp = ((ulong)subBuffer[i]) << (i * 8);
                number |= tmp;
            }

            return number;
        }

        public static ulong BytesToUInt64Calc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            ulong number = 0;

            for (int i = 0; i < 8; i++)
            {
                // ulong casting, if was possible, would be ok as well
                // needs BigInteger pow function because double can not precisely express 256 to the power of 7
                number = subBuffer[i] * (ulong)BigInteger.Pow(256, i);
            }

            return number;
        }

        public static byte[] SIntegerToBytes(BigInteger number, int intendedBytelen, bool littleEndian)
        {
            bool isNegative = false;
            List<byte> lst = new List<byte>();

            if (number < 0)
            {
                /*
                alternative code:
                number = number.add(BigInteger.ONE);
                number = number.multiply(BigInteger.valueOf(-1));
                 */
                isNegative = true;
                number = BigInteger.Negate(number);
            }

            BigInteger timesForNextDigit = number;
            byte remainderForCurrentDigit;

            do
            {
                remainderForCurrentDigit = (byte)BigInteger.Remainder(timesForNextDigit, 256);
                timesForNextDigit = BigInteger.Divide(timesForNextDigit, 256);

                if (isNegative)
                {
                    remainderForCurrentDigit = (byte)~remainderForCurrentDigit;
                }

                lst.Add(remainderForCurrentDigit);
            } while (timesForNextDigit != 0);

            if (intendedBytelen > lst.Count)
            {
                for (int i = 1; i < intendedBytelen - lst.Count; i++)
                {
                    lst.Add(0);
                }
            }

            byte[] buffer = lst.ToArray();

            if (littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static byte[] UIntegerToBytes(BigInteger number, int intendedBytelen, bool littleEndian)
        {
            List<byte> lst = new List<byte>();
            BigInteger timesForNextDigit = number;
            byte remainderForCurrentDigit;

            do
            {
                remainderForCurrentDigit = (byte)BigInteger.Remainder(timesForNextDigit, 256);
                timesForNextDigit = BigInteger.Divide(timesForNextDigit, 256);
                lst.Add(remainderForCurrentDigit);
            } while (timesForNextDigit != 0);

            if (intendedBytelen > lst.Count)
            {
                for (int i = 1; i < intendedBytelen - lst.Count; i++)
                {
                    lst.Add(0);
                }
            }

            byte[] buffer = lst.ToArray();

            if (littleEndian)
            {
                buffer = BitUtil.ReverseBuffer(buffer);
            }

            return buffer;
        }

        public static BigInteger BytesToSInteger(byte[] buffer, int offset, int bytelen, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[bytelen];
            Array.Copy(buffer, offset, subBuffer, 0, bytelen);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            BigInteger number = 0;
            bool isNegative = false;

            if (BitUtil.ByteToUByte(subBuffer[bytelen - 1]) >= 128)
            {
                isNegative = true;
            }

            for (int i = 0; i < bytelen; i++)
            {
                if (isNegative)
                {
                    subBuffer[i] = (byte)~subBuffer[i];
                }

                number = BigInteger.Multiply(new BigInteger(subBuffer[i]), BigInteger.Pow(256, bytelen));
            }

            if (isNegative)
            {
                /*
                alternative code:
                number = number.multiply(BigInteger.valueOf(-1));
                number = number.subtract(BigInteger.valueOf(1));
                 */
                number = BigInteger.Negate(number);
            }

            return number;
        }

        public static BigInteger BytesToUInteger(byte[] buffer, int offset, int bytelen, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[bytelen];
            Array.Copy(buffer, offset, subBuffer, 0, bytelen);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            BigInteger number = 0;

            for (int i = 0; i < bytelen; i++)
            {
                number = BigInteger.Multiply(new BigInteger(subBuffer[i]), BigInteger.Pow(256, bytelen));
            }

            return number;
        }

        public static byte[] DoubleToBytes(double d)
        {
            long _d = BitConverter.DoubleToInt64Bits(d);
            byte[] buffer = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                byte b = (byte)(_d >> ((7 - i) * 8));
                buffer[i] = b;
            }

            return buffer;
        }

        public static byte[] DoubleToBytesCalc(double d, bool littleEndian)
        {
            if (Equals(d, 0))
            {
                return new byte[8];
            }
            else
            {
                byte[] buffer;

                if (double.IsPositiveInfinity(d))
                {
                    buffer = new byte[8];
                    buffer[0] = 127;
                    buffer[1] = 240;

                    for (int i = 2; i < 8; i++)
                    {
                        buffer[i] = 0;
                    }
                }
                else if (double.IsNegativeInfinity(d))
                {
                    buffer = new byte[8];
                    buffer[0] = 255;
                    buffer[1] = 240;

                    for (int i = 2; i < 8; i++)
                    {
                        buffer[i] = 0;
                    }
                }
                else
                {
                    bool isNegative = false;
                    string binary = "0";
                    binary = BaseConverter.Convert(d.ToString(), 10, 2, 32, 1076);

                    if (StringMath.is_negative(binary))
                    {
                        isNegative = true;
                        binary = StringMath.switch_sign(binary);
                    }

                    string integer_part = StringMath.get_integer_part(binary);
                    string fraction_part = StringMath.get_fraction_part(binary);
                    short exponent = 0;

                    if (!StringMath.are_equal(integer_part, "0"))
                    {
                        exponent = (short)(integer_part.Length - 1);
                        binary = integer_part.Substring(1) + fraction_part;
                    }
                    else
                    {
                        exponent--;

                        for (int i = 0; i < fraction_part.Length; i++)
                        {
                            if (fraction_part[i] != '0')
                            {
                                binary = fraction_part.Substring(i + 1);
                                break;
                            }
                            else
                            {
                                exponent--;
                            }
                        }
                    }

                    exponent += 1023;
                    string _exponent = "0";
                    _exponent = BaseConverter.Convert(exponent.ToString(), 10, 2, 0, 0);
                    _exponent = StringUtil.PrependChar(_exponent, "0", 11 - _exponent.Length);
                    buffer = new byte[8];
                    buffer[0] = BitUtil.SetBit(0, (int)BitUtil.BITS.EIGHTH, isNegative);
                    int bitpos = 6;
                    int bytepos = 0;
                    buffer = BitUtil.WriteBits(buffer, ref bytepos, ref bitpos, _exponent, true);
                    int padding = 52 - binary.Length;

                    if (padding < 0)
                    {
                        binary = binary.Substring(0, 52);
                        binary = BaseConverter.Convert(binary, 2, 10, 0, 0);
                        binary = StringMath.add(new List<string> { binary, "1" });
                        binary = BaseConverter.Convert(binary, 10, 2, 0, 0);
                        padding = 52 - binary.Length;
                        binary = StringUtil.PrependChar(binary, "0", padding);
                    }
                    else
                    {
                        binary = StringUtil.AppendChar(binary, "0", padding);
                    }

                    buffer = BitUtil.WriteBits(buffer, ref bytepos, ref bitpos, binary, true);
                }

                if (!littleEndian)
                {
                    buffer = BitUtil.ReverseBuffer(buffer);
                }

                return buffer;
            }
        }

        public static double BytesToDouble(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            double d = 0;

            for (int i = 0; i < 8; i++)
            {
                long l = (long)subBuffer[i] << ((7 - i) * 8);
                d = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(d) | l);
            }

            return d;
        }

        public static double BytesToDoubleCalc(byte[] buffer, int offset, bool isLittleEndian)
        {
            byte[] subBuffer = new byte[8];
            Array.Copy(buffer, offset, subBuffer, 0, 8);

            if (BytesToInt64(subBuffer, 0, true) == 0)
            {
                return 0;
            }

            if (!isLittleEndian)
            {
                subBuffer = BitUtil.ReverseBuffer(subBuffer);
            }

            byte tmp = subBuffer[0];
            byte sign = (byte)(tmp >> 7);
            int exp_upper7 = tmp << 4;
            tmp = subBuffer[1];
            int exp_lower4 = tmp >> 4;
            int exponent = exp_upper7 | exp_lower4;
            int _exponent = exponent - 1023;
            int bytepos = 1;
            int bitpos = 3;
            string binary = (String)BitUtil.ReadBits(subBuffer, ref bytepos, ref bitpos, 52, true);

            if (exponent == 2047)
            {
                if (StringMath.are_equal(binary, "0"))
                {
                    if (sign == 1)
                    {
                        return double.NegativeInfinity;
                    }
                    else
                    {
                        return double.PositiveInfinity;
                    }
                }
                else
                {
                    return double.NaN;
                }
            }
            else
            {
                binary = StringMath.beautify_number("1." + binary);
                double dec;
                dec = Convert.ToDouble(BaseConverter.Convert(binary, 2, 10, 0, 31));
                dec *= 1 - (2 * sign);
                dec *= Math.Pow(2, _exponent);
                return dec;
            }
        }

        public static string VLERead(byte[] buffer, ref int offset, bool isLittleEndian)
        {
            int bytelen = 0;
            bool readNextByte;
            List<string> strBytes = new List<string>();

            do
            {
                byte b = buffer[offset];
                readNextByte = BitUtil.GetBit(b, BitUtil.BITS.EIGHTH);
                string strB = BitUtil.byteToString(b).Substring(1);
                strBytes.Add(strB);
                bytelen++;
                offset++;
            } while (readNextByte && bytelen < 5);

            if (!isLittleEndian)
            {
                strBytes.Reverse();
            }

            return string.Join("", strBytes);
        }

        public static BigInteger VLEURead(byte[] buffer, ref int offset, bool isLittleEndian)
        {
            string read = VLERead(buffer, ref offset, isLittleEndian);
            string reverse = StringUtil.Reverse(read);
            return BigInteger.Parse(BaseConverter.Convert(reverse, 2, 10, 0, 0));
        }
    }
}