using System.Numerics;
using System.Runtime.CompilerServices;
using ZeroX.Utilities;
using Nethereum.Hex.HexConvertors.Extensions;

[assembly: InternalsVisibleTo("Tests")]


namespace ZeroX.Transactions
{
    public class Transaction
    {

        public BigInteger Salt { get; }
        public EthereumAddress SignerAddress { get; }
        public string TxData { get; }

        internal Transaction(EthereumAddress signerAddress, string txData)
        {
            Salt = 0; // Make random
            SignerAddress = signerAddress;
            TxData = txData;
        }

        internal TransactionEIP712 EIP712Transaction
        {
            get => new TransactionEIP712
            {
                Salt = Salt,
                SignerAddress = SignerAddress,
                Data = TxData.HexToByteArray()
            };
        }

        public byte[] Hash(EthereumAddress exchangeAddress)
        {
            var hash = EIP712.EIP712.Hash(EIP712Transaction, new EIP712.EIP712Domain
            {
                Name = "0x Protocol",
                Version = "2",
                VerifyingContract = exchangeAddress
            });

            return hash;
        }

    }
}
