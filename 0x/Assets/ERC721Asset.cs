
using Nethereum.ABI;
using Nethereum.ABI.Encoders;
using Nethereum.Util;
using System;
using System.Linq;
using System.Numerics;
using ZeroX.Utilities;

namespace ZeroX.Assets
{

    /// <summary>
    /// ERC721 Asset
    /// </summary>
    public class ERC721Asset : TokenAsset
    {

        private static readonly byte[] _erc721AssetHeader = new byte[] { 0x02, 0x57, 0x17, 0x92 };
        private const int AssetDataLength = 68;

        private static byte[] EncodeAssetData(EthereumAddress tokenAddress, BigInteger tokenId)
            => ByteUtil.Merge(_erc721AssetHeader, 
                EncodeAddress(tokenAddress), new IntTypeEncoder().Encode(tokenId));

        private ERC721Asset(byte[] assetData)
            : base(assetData, EthereumAddress.FromByteArray(assetData.Slice(16, 36)))
        {
            // Reversing due to fact that BigInteger expects little endian order
            TokenId = new BigInteger(assetData.Slice(36).Reverse().ToArray());
        }

        private ERC721Asset(EthereumAddress tokenAddress, BigInteger tokenId)
            : base(EncodeAssetData(tokenAddress, tokenId), tokenAddress) { }

        public BigInteger TokenId { get; }

        /// <summary>
        /// Creates an ERC721 asset
        /// </summary>
        /// <param name="tokenAddress">Address of ERC721 token</param>
        /// <param name="tokenId">Token id to exchange</param>
        /// <returns><see cref="ERC721Asset"/> representing ERC721 asset</returns>
        /// <exception cref="ArgumentNullException">tokenAddress is null</exception>
        /// <exception cref="ArgumentException">tokenId invalid value</exception>
        public static ERC721Asset Create(EthereumAddress tokenAddress, BigInteger tokenId)
        {
            tokenAddress = tokenAddress ?? throw new ArgumentNullException(nameof(tokenAddress));
            if (tokenId > IntType.MAX_UINT256_VALUE || tokenId < 0)
                throw new ArgumentException("Token id invalid value", nameof(tokenId));

            return new ERC721Asset(tokenAddress, tokenId);
        }

        /// <summary>
        /// Creates an ERC721 asset from ERC721 compatible asset data
        /// </summary>
        /// <param name="assetData">ERC721 asset data</param>
        /// <returns><see cref="ERC721Asset"/> representing ERC721 asset</returns>
        /// <exception cref="ArgumentNullException">assetData is null</exception>
        /// <exception cref="ArgumentException">asset data invalid value</exception>
        public static ERC721Asset Create(byte[] assetData)
        {
            assetData = assetData ?? throw new ArgumentNullException(nameof(assetData));

            if (assetData.Length != 68 || !assetData.Slice(0, 4).SequenceEqual(_erc721AssetHeader)
                || !ValidateTokenId(new BigInteger(assetData.Slice(36))))
                throw new ArgumentException("Asset data not valid");

            return new ERC721Asset(assetData);
        }

        private static bool ValidateTokenId(BigInteger tokenId)
            => tokenId <= IntType.MAX_UINT256_VALUE && tokenId >= 0;

        public static bool ValidateAssetData(byte[] assetData)
            => assetData.Length == 68 
            && assetData.Slice(0, 4).SequenceEqual(_erc721AssetHeader)
            && ValidateTokenId(new BigInteger(assetData.Slice(36)));
    }
}
