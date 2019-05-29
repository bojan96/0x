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

        /// <summary>
        /// Constructs instance of TxParameters class
        /// </summary>
        /// <param name="gasPrice">Gas price of transaction, set to null to use contract default values</param>
        /// <param name="gasLimit">Gas limit of transaction, set to null to use gas estimation</param>
        /// <param name="nonce">Transaction nonce, set to null to get next nonce value from connected node</param>
        public TxParameters(BigInteger? gasPrice, BigInteger? gasLimit, BigInteger? nonce)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
            Nonce = nonce;
        }

        /// <summary>
        /// Constructs instance of TxParameters class
        /// </summary>
        /// <param name="gasPrice">Gas price of transaction, set to null to use contract default values</param>
        /// <param name="gasLimit">Gas limit of transaction, set to null to use gas estimation</param>
        public TxParameters(BigInteger? gasPrice = null, BigInteger? gasLimit = null)
        {
            GasPrice = gasPrice;
            GasLimit = gasLimit;
        }

        /// <summary>
        /// Constructs instance of TxParameters class
        /// </summary>
        /// <param name="gasLimit">Gas limit of transaction, set to null to use gas estimation</param>
        public TxParameters(BigInteger? gasLimit)
        {
            GasLimit = gasLimit;
        }

        private BigInteger? _gasPrice = null;
        private BigInteger? _gasLimit = null;
        private BigInteger? _nonce = null;

        /// <summary>
        /// Transaction gas price
        /// </summary>
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

        /// <summary>
        /// Transaction gas limit
        /// </summary>
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


        /// <summary>
        /// Transaction nonce
        /// </summary>
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
