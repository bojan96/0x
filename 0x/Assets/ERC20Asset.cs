using Nethereum.Util;
using System;
using ZeroX.Utilities;

namespace ZeroX.Assets
{
    public class ERC20Asset : TokenAsset
    {

        private static readonly byte[] _erc20AssetHeader = new byte[] { 0xf4, 0x72, 0x61, 0xb0 };

        private static byte[] EncodeAssetData(EthereumAddress tokenAddress)
            // 12 zero bytes to pad address to 32 bytes
            => ByteUtil.Merge(_erc20AssetHeader, EncodeAddress(tokenAddress));

        private ERC20Asset(EthereumAddress tokenAddress)
            : base(EncodeAssetData(tokenAddress), tokenAddress) { }

        /// <summary>
        /// Creates an ERC20 asset
        /// </summary>
        /// <param name="tokenAddress">Address of erc20 token</param>
        /// <returns></returns>
        public static ERC20Asset Create(EthereumAddress tokenAddress)
            => new ERC20Asset(tokenAddress ?? throw new ArgumentNullException(nameof(tokenAddress)));
    }
}
