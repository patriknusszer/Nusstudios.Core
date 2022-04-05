using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System;

namespace Nusstudios.Reference.System.Security.Cryptography {
	/// <summary>
	/// Computes the <see cref="T:System.Security.Cryptography.SHA3" /> hash for the input data.
	/// </summary>
	[ComVisible(true)]
	public abstract class Keccak : HashAlgorithm {

		static Keccak() {
			CryptoConfig.AddAlgorithm(typeof(KeccakManaged), "Keccak", "KeccakManaged", "Keccak", "System.Security.Cryptography.Keccak");
			CryptoConfig.AddOID("0.9.2.0", "Keccak", "KeccakManaged", "Keccak", "System.Security.Cryptography.Keccak");
		}

		/// <summary>
		/// Initializes a new instance of <see cref="T:System.Security.Cryptography.SHA3" />.
		/// </summary>
		protected Keccak() {
			base.HashSizeValue = 512;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hashBitLength"></param>
        protected Keccak(int hashBitLength, bool useStandardPadding) {
			if (hashBitLength != 224 && hashBitLength != 256 && hashBitLength != 384 && hashBitLength != 512)
				throw new ArgumentException("hashBitLength must be 224, 256, 384, or 512", "hashBitLength");
			Initialize();
			HashSizeValue = hashBitLength;
			switch (hashBitLength) {
				case 224:
					KeccakR = 1152;
					break;
				case 256:
					KeccakR = 1088;
					break;
				case 384:
					KeccakR = 832;
					break;
				case 512:
					KeccakR = 576;
					break;
			}
			RoundConstants = new ulong[]
            {
                0x0000000000000001UL,
                0x0000000000008082UL,
                0x800000000000808aUL,
                0x8000000080008000UL,
                0x000000000000808bUL,
                0x0000000080000001UL,
                0x8000000080008081UL,
                0x8000000000008009UL,
                0x000000000000008aUL,
                0x0000000000000088UL,
                0x0000000080008009UL,
                0x000000008000000aUL,
                0x000000008000808bUL,
                0x800000000000008bUL,
                0x8000000000008089UL,
                0x8000000000008003UL,
                0x8000000000008002UL,
                0x8000000000000080UL,
                0x000000000000800aUL,
                0x800000008000000aUL,
                0x8000000080008081UL,
                0x8000000000008080UL,
                0x0000000080000001UL,
                0x8000000080008008UL
            };
		}

		/// <summary>Creates an instance of the default implementation of <see cref="T:System.Security.Cryptography.SHA3" />.</summary>
		/// <returns>A new instance of <see cref="T:System.Security.Cryptography.SHA3" />.</returns>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		/// <PermissionSet>
		/// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
		public static new Keccak Create() {
			return Create("System.Security.Cryptography.Keccak");
		}

		/// <summary>Creates an instance of a specified implementation of <see cref="T:System.Security.Cryptography.SHA3" />.</summary>
		/// <returns>A new instance of <see cref="T:System.Security.Cryptography.SHA3" /> using the specified implementation.</returns>
		/// <param name="hashName">The name of the specific implementation of <see cref="T:System.Security.Cryptography.SHA3" /> to be used. </param>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm described by the <paramref name="hashName" /> parameter was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		public static new Keccak Create(string hashName) {
			return (Keccak)CryptoConfig.CreateFromName(hashName);
		}

		#region " Hash Algorithm Members "
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="ibStart"></param>
		/// <param name="cbSize"></param>
		protected override void HashCore(byte[] array, int ibStart, int cbSize) {
			if (array == null)
				throw new ArgumentNullException("array");
			if (ibStart < 0)
				throw new ArgumentOutOfRangeException("ibStart");
			if (cbSize > array.Length)
				throw new ArgumentOutOfRangeException("cbSize");
			if (ibStart + cbSize > array.Length)
				throw new ArgumentOutOfRangeException("ibStart or cbSize");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override byte[] HashFinal() {
			return this.Hash;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Initialize() {
			buffLength = 0;
			state = new ulong[5 * 5];//1600 bits
			HashValue = null;
		}
		#endregion

		#region Implementation
		internal const int KeccakB = 1600;
		internal const int KeccakNumberOfRounds = 24;
		internal const int KeccakLaneSizeInBits = 8 * 8;
		internal readonly ulong[] RoundConstants;
		internal ulong[] state;
		internal byte[] buffer;
		internal int buffLength;
		//protected new byte[] HashValue;
		//protected new int HashSizeValue;
		internal int keccakR;
        internal bool useStandardPadding = true;

		internal int KeccakR {
			get {
				return keccakR;
			}
			set {
				keccakR = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int SizeInBytes {
			get {
				return KeccakR / 8;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int HashByteLength {
			get {
				return HashSizeValue / 8;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanReuseTransform {
			get {
				return true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		protected ulong ROL(ulong a, int offset) {
			return (((a) << ((offset) % KeccakLaneSizeInBits)) ^ ((a) >> (KeccakLaneSizeInBits - ((offset) % KeccakLaneSizeInBits))));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		protected void AddToBuffer(byte[] array, ref int offset, ref int count) {
			int amount = Math.Min(count, buffer.Length - buffLength);
			Buffer.BlockCopy(array, offset, buffer, buffLength, amount);
			offset += amount;
			buffLength += amount;
			count -= amount;
		}

		/// <summary>
		/// 
		/// </summary>
		public override byte[] Hash {
			get {
				return HashValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override int HashSize {
			get {
				return HashSizeValue;
			}
		}
		#endregion

	}
}
