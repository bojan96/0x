using Nethereum.Util;
using System;
using System.Linq;
using ZeroX.Utilities;

namespace ZeroX.Assets
{

    /// <summary>
    /// ERC20 asset
    /// </summary>
    public class ERC20Asset : TokenAsset
    {

        private static readonly byte[] _erc20AssetHeader = new byte[] { 0xf4, 0x72, 0x61, 0xb0 };

        private static byte[] EncodeAssetData(EthereumAddress tokenAddress)
            => ByteUtil.Merge(_erc20AssetHeader, EncodeAddress(tokenAddress));

        private ERC20Asset(EthereumAddress tokenAddress)
            : base(EncodeAssetData(tokenAddress), tokenAddress) { }

        private ERC20Asset(byte[] assetData)
            : base(assetData, EthereumAddress.FromByteArray(assetData.Slice(16))) { }

        /// <summary>
        /// Creates an ERC20 asset
        /// </summary>
        /// <param name="tokenAddress">Address of ERC20 token</param>
        /// <returns><see cref="ERC20Asset"/> representing ERC20 asset</returns>
        /// <exception cref="ArgumentNullException">tokenAddress is null</exception>
        public static ERC20Asset Create(EthereumAddress tokenAddress)
            => new ERC20Asset(tokenAddress ?? throw new ArgumentNullException(nameof(tokenAddress)));

        /// <summary>
        /// Creates an ERC20 asset from ERC20 compatible asset data
        /// </summary>
        /// <param name="assetData">ERC20 asset data</param>
        /// <returns><see cref="ERC20Asset"/> representing ERC20 asset</returns>
        /// <exception cref="ArgumentNullException">assetData is null</exception>
        /// <exception cref="ArgumentException">Asset data is invalid</exception>
        public static ERC20Asset Create(byte[] assetData)
        {
            if (assetData == null)
                throw new ArgumentNullException(nameof(assetData));

            if (assetData.Length != 36 || ! assetData.Slice(0, 4).SequenceEqual(_erc20AssetHeader))
                throw new ArgumentException("Asset data not valid", nameof(assetData));

            return new ERC20Asset(assetData);
        }
    }
}
