using System.Numerics;

namespace ZeroX.Utilities
{
    // TODO: Add argument validation
    public class TxParameters
    {
        public TxParameters(BigInteger gasPrice, int gasLimit, BigInteger nonce)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
            Nonce = nonce;
        }
        public BigInteger GasPrice { get; set; } = - 1;
        public int GasLimit { get; set; } = -1;
        public BigInteger Nonce { get; set; } = -1;
    }
}
