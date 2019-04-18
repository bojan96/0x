using Nethereum.Util;
using System.Text;
using ZeroX.Utilities;

namespace ZeroX
{
    public class ERC20Asset : Asset
    {
        public ERC20Asset(EthereumAddress tokenAddress)
            : base(tokenAddress, _erc20AssetHeader) { }
    }
}
