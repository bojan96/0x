using Nethereum.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using System;

namespace ZeroX.Utilities
{
    public class Account
    {
        /// <summary>
        /// Creates ethereum account from private key
        /// </summary>
        /// <param name="privateKey"></param>
        public Account(string privateKey)
        {
            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));
            if (!ValidatePrivateKey(privateKey))
                throw new ArgumentException("Invalid private key", nameof(privateKey));
            PrivateKey = privateKey;
            Address = (EthereumAddress)Web3.GetAddressFromPrivateKey(privateKey);
        }

        public string PrivateKey { get; }
        public EthereumAddress Address { get; }

        private bool ValidatePrivateKey(string privateKey)
        {
            try
            {
                byte[] privateKeyByte = privateKey.HexToByteArray();
                return privateKeyByte.Length == 32;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
