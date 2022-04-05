using System.Security.Cryptography;
using System;

namespace Nusstudios.Reference.System.Security.Cryptography {
	/// <summary>
	/// 
	/// </summary>
	public class HMACSHA3 : HMAC {

		static HMACSHA3() {
			Keccak sha = Keccak.Create();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hashBitLength"></param>
		public HMACSHA3(int hashBitLength = 512) : this(Utils.GenerateRandom(0x80), hashBitLength) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="hashBitLength"></param>
		public HMACSHA3(byte[] key, int hashBitLength = 512) {
			base.HashName = "KeccakManaged";
			SetHashBitLength(hashBitLength);
			SetHMACBlockSize();
			Initialize();
			base.Key = (byte[])key.Clone();
		}

		private void SetHMACBlockSize() {
			switch (base.HashSizeValue) {
				case 224:
					base.BlockSizeValue = 144;
					break;
				case 256:
					base.BlockSizeValue = 136;
					break;
				case 384:
					base.BlockSizeValue = 104;
					break;
				case 512:
					base.BlockSizeValue = 72;
					break;
			}
		}

		private void SetHashBitLength(int hashBitLength) {
			if (hashBitLength != 512) throw new NotImplementedException("HMAC-SHA3 is only implemented for 512bits hashes.");
			if (hashBitLength != 224 && hashBitLength != 256 && hashBitLength != 384 && hashBitLength != 512)
				throw new ArgumentException("Hash bit length must be 224, 256, 384, or 512", "hashBitLength");
			base.HashSizeValue = hashBitLength;
		}
	}
}
