using Nethereum.ABI.Encoders;
using Nethereum.Util;
using ZeroX.Utilities;

namespace ZeroX.Assets
{
    public abstract class TokenAsset : Asset
    {

        protected static byte[] EncodeAddress(EthereumAddress address)
            => new AddressTypeEncoder().Encode(address.ToString());

        protected TokenAsset(byte[] assetData, EthereumAddress tokenAddress)
            : base(assetData) => TokenAddress = tokenAddress;

        public EthereumAddress TokenAddress { get; }
    }
}
