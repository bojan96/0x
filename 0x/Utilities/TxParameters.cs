using System.Numerics;

namespace ZeroX.Utilities
{
    // TODO: Add argument validation
    public class TxParameters
    {
        public TxParameters(BigInteger gasPrice, int gasLimit)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
        }
        public BigInteger GasPrice { get; set; }
        public int GasLimit { get; set; }
    }
}
