using ZeroX.Utilities;

namespace ZeroX.Assets
{
    public class ERC20Asset : Asset
    {
        public ERC20Asset(EthereumAddress tokenAddress)
            : base(tokenAddress, _erc20AssetHeader) { }
    }
}
