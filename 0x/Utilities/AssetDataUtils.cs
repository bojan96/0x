using Nethereum.Util;
using System;
using System.Text;

namespace ZeroX.Utilities
{
    public static class AssetDataUtils
    {

        private const string ERC20TokenSignature = "ERC20Token(address)";
        private static readonly byte[] _erc20AssetHeader = Sha3Keccack.Current
            .CalculateHash(Encoding.UTF8.GetBytes(ERC20TokenSignature)).Slice(0, 4);


        /// <summary>
        /// Encodes ERC20 contract address as per 0x specification
        /// </summary>
        /// <param name="address">Contract address</param>
        /// <returns>Asset data</returns>
        public static byte[] GetERC20AssetData(EthereumAddress address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            // Pad byte array to 32 bytes
            byte[] arr = ByteUtil.Merge(new byte[12], address.Bytes);
            return ByteUtil.Merge(_erc20AssetHeader, arr);
        }

    }
}
