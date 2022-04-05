using System;
using System.Security.Cryptography;

namespace Nusstudios.Reference.System.Security.Cryptography {
	internal static class Utils {
		private static volatile RNGCryptoServiceProvider _rng = null;

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//internal static extern bool _ProduceLegacyHmacValues();

		internal static RNGCryptoServiceProvider StaticRandomNumberGenerator {
			get {
				if (_rng == null) {
					_rng = new RNGCryptoServiceProvider();
				}
				return _rng;
			}
		}

		internal static byte[] GenerateRandom(int keySize) {
			byte[] data = new byte[keySize];
			StaticRandomNumberGenerator.GetBytes(data);
			return data;
		}

        internal static void DWORDToBigEndian(byte[] block, uint[] x, int digits)
        {
            int i;
            int j;

            for (i = 0, j = 0; i < digits; i++, j += 4)
            {
                block[j] = (byte)((x[i] >> 24) & 0xff);
                block[j + 1] = (byte)((x[i] >> 16) & 0xff);
                block[j + 2] = (byte)((x[i] >> 8) & 0xff);
                block[j + 3] = (byte)(x[i] & 0xff);
            }
        }

        internal unsafe static void DWORDFromBigEndian(uint* x, int digits, byte* block)
        {
            int i;
            int j;

            for (i = 0, j = 0; i < digits; i++, j += 4)
                x[i] = (uint)((block[j] << 24) | (block[j + 1] << 16) | (block[j + 2] << 8) | block[j + 3]);
        }

        internal unsafe static void QuadWordFromBigEndian(UInt64* x, int digits, byte* block)
        {
            int i;
            int j;

            for (i = 0, j = 0; i < digits; i++, j += 8)
                x[i] = (
                         (((UInt64)block[j]) << 56) | (((UInt64)block[j + 1]) << 48) |
                         (((UInt64)block[j + 2]) << 40) | (((UInt64)block[j + 3]) << 32) |
                         (((UInt64)block[j + 4]) << 24) | (((UInt64)block[j + 5]) << 16) |
                         (((UInt64)block[j + 6]) << 8) | ((UInt64)block[j + 7])
                        );
        }

        // encodes x (DWORD) into block (unsigned char), most significant byte first.
        // digits = number of QWORDS 
        internal static void QuadWordToBigEndian(byte[] block, UInt64[] x, int digits)
        {
            int i;
            int j;

            for (i = 0, j = 0; i < digits; i++, j += 8)
            {
                block[j] = (byte)((x[i] >> 56) & 0xff);
                block[j + 1] = (byte)((x[i] >> 48) & 0xff);
                block[j + 2] = (byte)((x[i] >> 40) & 0xff);
                block[j + 3] = (byte)((x[i] >> 32) & 0xff);
                block[j + 4] = (byte)((x[i] >> 24) & 0xff);
                block[j + 5] = (byte)((x[i] >> 16) & 0xff);
                block[j + 6] = (byte)((x[i] >> 8) & 0xff);
                block[j + 7] = (byte)(x[i] & 0xff);
            }
        }

        #region " Environment "
        //[MethodImpl(MethodImplOptions.InternalCall)]
        //internal static extern string GetResourceFromDefault(string key);
        ////[SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        //internal static string GetResourceString(string key) {
        //	return GetResourceFromDefault(key);
        //}
        #endregion
    }
}
