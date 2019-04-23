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
        public BigInteger GasPrice { get; set; }
        public int GasLimit { get; set; }
        public BigInteger Nonce { get; set; }
    }
}
