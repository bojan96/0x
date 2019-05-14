using System;
using System.Numerics;

namespace ZeroX.Utilities
{
    /// <summary>
    /// Ethereum transaction parameters
    /// </summary>
    public class TxParameters
    {
        private const int MinGasLimit = 21000;

        public TxParameters(BigInteger gasPrice, int gasLimit, BigInteger nonce)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
            Nonce = nonce;
        }

        public TxParameters(BigInteger gasPrice, int gasLimit)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
        }

        public TxParameters(int gasLimit)
        {
            GasLimit = gasLimit;
        }

        private BigInteger? _gasPrice = null;
        private BigInteger? _gasLimit = null;
        private BigInteger? _nonce = null;

        public BigInteger? GasPrice
        {
            get => _gasPrice;
            set
            {
                if (value != null)
                    _gasPrice = value >= 0 ? value :
                        throw new ArgumentException("Gas price value can not be negative", nameof(GasPrice));
                else
                    _gasPrice = null;
            }
        } 

        public BigInteger? GasLimit
        {
            get => _gasLimit;
            set
            {
                if (value != null)
                    _gasLimit = value >= MinGasLimit ? value :
                        throw new ArgumentException("Gas limit value can not be negative", nameof(GasLimit));
                else
                    _gasLimit = null;
            }
        }

        public BigInteger? Nonce
        {
            get => _nonce;
            set
            {
                if (value != null)
                    _nonce = _nonce >= 0 ? value :
                        throw new ArgumentException("Nonce value can not be negative", nameof(Nonce));
                else
                    _nonce = null;
            }
        }
    }
}
