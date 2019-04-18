using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System;
using System.Diagnostics;
using ZeroX.Utilities;

namespace ZeroX.Assets
{
    public abstract class Asset
    {

        protected static readonly byte[] _erc20AssetHeader = new byte[] { 0xf4, 0x72, 0x61, 0xb0 };
        protected static readonly byte[] _erc721AssetHeader = new byte[] { 0x02, 0x57, 0x17, 0x92 };

        public EthereumAddress TokenAddress { get; }

        public byte[] AssetProxyId { get; }
        public byte[] AssetData { get; }

        internal Asset(EthereumAddress tokenAddress, byte[] assetProxyId)
        {
            Debug.Assert(assetProxyId.Length == 4);
            TokenAddress = tokenAddress ?? throw new ArgumentNullException(nameof(tokenAddress));
            AssetProxyId = assetProxyId;

            // Pad address to 32 bytes
            AssetData = ByteUtil.Merge(assetProxyId, new byte[12], TokenAddress.Bytes);
        }

        public override string ToString()
            => AssetData.ToHex();
    }
}
