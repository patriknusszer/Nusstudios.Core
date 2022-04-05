using System;
using System.Collections.Generic;

namespace Nusstudios.Core
{
    public static class HashCore
    {
        internal const Int32 HashPrime = 101;

        // Table of prime numbers to use as hash table sizes. 
        // A typical resize algorithm would pick the smallest prime number in this array
        // that is larger than twice the previous capacity. 
        // Suppose our Hashtable currently has capacity x and enough elements are added 
        // such that a resize needs to occur. Resizing first computes 2x then finds the 
        // first prime in the table greater than 2x, i.e. if primes are ordered 
        // p_1, p_2, ..., p_i, ..., it finds p_n such that p_n-1 < 2x < p_n. 
        // Doubling is important for preserving the asymptotic complexity of the 
        // hashtable operations such as add.  Having a prime guarantees that double 
        // hashing does not lead to infinite loops.  IE, your hash function will be 
        // h1(key) + i*h2(key), 0 <= i < size.  h2 and the size must be relatively prime.
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public static int GetPrime(int min)
        {
            if (min < 0)
                throw new ArgumentException();

            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < Int32.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % 101 != 0))
                    return i;
            }
            return min;
        }

        public static int GetMinPrime()
        {
            return primes[0];
        }

        // Returns size of hashtable to grow to.
        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                // Contract.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
                return MaxPrimeArrayLength;
            }

            return GetPrime(newSize);
        }


        // This is the maximum prime smaller than Array.MaxArrayLength
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        public static uint RolloutHash32<TIn>(TIn[] coll, int prime)
        {
            uint hashCode = 0;

            for (int x = -1; ++x < coll.Length;)
            {
                TIn elem = coll[x];

                if (elem is char c) hashCode += c * (uint)Math.Pow(prime, x);
                if (elem is string str) hashCode += RolloutHash32(str, prime) % UInt32.MaxValue;
                else if (elem is byte b) hashCode += b * (uint)Math.Pow(prime, x);
                else if (elem is sbyte sb) hashCode += (uint)(sb * (uint)Math.Pow(prime, x));
                else if (elem is ushort us) hashCode += us * (uint)Math.Pow(prime, x);
                else if (elem is short s) hashCode += (uint)(s * (uint)Math.Pow(prime, x));
                else if (elem is uint ui) hashCode += ui * (uint)Math.Pow(prime, x);
                else if (elem is int i) hashCode += (uint)(i * (uint)Math.Pow(prime, x));
                else if (elem is ulong ul) hashCode += (uint)(ul * (ulong)Math.Pow(prime, x) % UInt32.MaxValue);
                else if (elem is long l) hashCode += (uint)((ulong)l * (ulong)Math.Pow(prime, x) % UInt32.MaxValue);
                else if (elem is double d) hashCode += (uint)((ulong)BitConverter.DoubleToInt64Bits(d) * (ulong)Math.Pow(prime, x) % UInt32.MaxValue);
                else if (elem is float f) hashCode += BitConverter.ToUInt32(BitConverter.GetBytes(f), 0) * (uint)Math.Pow(prime, x);
                else throw new Exception("Type not supported");
            }

            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<double> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (double e in coll) hashCode += (uint)((ulong)BitConverter.DoubleToInt64Bits(e) * (ulong)Math.Pow(prime, x++) % UInt32.MaxValue);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<float> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (double e in coll) hashCode += BitConverter.ToUInt32(BitConverter.GetBytes(e), 0) * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<string> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (string e in coll) hashCode += RolloutHash32(e, prime) * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<char> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (char e in coll) hashCode += e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<byte> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (byte e in coll) hashCode += e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<sbyte> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (sbyte e in coll) hashCode += (uint)e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }


        public static uint RolloutHash32(IEnumerable<ushort> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (short e in coll) hashCode += (uint)e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<short> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (short e in coll) hashCode += (uint)e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<uint> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (uint e in coll) hashCode += e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<int> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (int e in coll) hashCode += (uint)e * (uint)Math.Pow(prime, x++);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<ulong> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (ulong e in coll) hashCode += (uint)(e * (ulong)Math.Pow(prime, x++) % UInt32.MaxValue);
            return hashCode;
        }

        public static uint RolloutHash32(IEnumerable<long> coll, int prime)
        {
            uint hashCode = 0;
            int x = 0;
            foreach (long e in coll) hashCode += (uint)((ulong)e * (ulong)Math.Pow(prime, x++) % UInt32.MaxValue);
            return hashCode;
        }

        public static uint RolloutHash32(byte[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += coll[x] * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(string coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += coll[x] * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(sbyte[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)coll[x] * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(uint[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += coll[x] * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(int[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)coll[x] * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(ulong[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)(coll[x] * (ulong)Math.Pow(prime, x) % Int32.MaxValue);
            return hashCode;
        }

        public static uint RolloutHash32(long[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)((ulong)coll[x] * (ulong)Math.Pow(prime, x) % Int32.MaxValue);
            return hashCode;
        }

        public static uint RolloutHash32(double[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)BitConverter.DoubleToInt64Bits(coll[x]) * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static uint RolloutHash32(float[] coll, int prime)
        {
            uint hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += (uint)BitConverter.ToInt32(BitConverter.GetBytes(coll[x]), 0) * (uint)Math.Pow(prime, x);
            return hashCode;
        }

        public static ulong RolloutHash64<TIn>(TIn[] coll, int prime)
        {
            ulong hashCode = 0;

            for (int x = -1; ++x < coll.Length;)
            {
                TIn elem = coll[x];

                if (elem is char c) hashCode += c * (ulong)Math.Pow(prime, x);
                if (elem is string str) hashCode += RolloutHash64(str, prime);
                else if (elem is byte b) hashCode += b * (ulong)Math.Pow(prime, x);
                else if (elem is sbyte sb) hashCode += (ulong)sb * (ulong)Math.Pow(prime, x);
                else if (elem is ushort us) hashCode += us * (ulong)Math.Pow(prime, x);
                else if (elem is short s) hashCode += (ulong)s * (ulong)Math.Pow(prime, x);
                else if (elem is uint ui) hashCode += ui * (ulong)Math.Pow(prime, x);
                else if (elem is int i) hashCode += (ulong)i * (ulong)Math.Pow(prime, x);
                else if (elem is ulong ul) hashCode += ul * (ulong)Math.Pow(prime, x);
                else if (elem is long l) hashCode += (ulong)l * (ulong)Math.Pow(prime, x);
                else if (elem is double d) hashCode += (ulong)BitConverter.DoubleToInt64Bits(d) * (ulong)Math.Pow(prime, x);
                else if (elem is float f) hashCode += (ulong)BitConverter.ToUInt32(BitConverter.GetBytes(f), 0) * (ulong)Math.Pow(prime, x);
                else throw new Exception("Type not supported");
            }

            return hashCode;
        }

        public static ulong RolloutHash64(string coll, int prime)
        {
            ulong hashCode = 0;
            for (int x = -1; ++x < coll.Length;) hashCode += coll[x] * (ulong)Math.Pow(prime, x);
            return hashCode;
        }

        public static ulong RolloutHash64(IEnumerable<string> coll, int prime)
        {
            ulong hashCode = 0;
            int x = 0;
            foreach (string e in coll) hashCode += RolloutHash64(e, prime) * (ulong)Math.Pow(prime, x++);
            return hashCode;
        }

        public static ulong Mix64(ulong hash, ulong rnd)
        {
            hash += rnd;
            hash += 0x2A54685045449290;
            hash = hash ^ (hash >> 30) * 0x5116B3F544BBB6FF;
            hash = hash ^ (hash >> 27) * 0xB2B1575A84A75C64;
            return hash ^ (hash >> 31);
        }
    }
}
