using System.Numerics;
using System.Runtime.CompilerServices;
using ZeroX.Utilities;
using Nethereum.Hex.HexConvertors.Extensions;
using EIP712;
using System;
using Nethereum.Util;
using Random = ZeroX.Utilities.Random;

[assembly: InternalsVisibleTo("Tests")]


namespace ZeroX.Transactions
{
    public class Transaction
    {

        public BigInteger Salt { get; }
        public EthereumAddress SignerAddress { get; }
        public byte[] TxData { get; }

        internal Transaction(EthereumAddress signerAddress, string txData)
        {
            Salt = Random.GenerateSalt();
            SignerAddress = signerAddress;
            TxData = txData.HexToByteArray();
        }

        internal TransactionEIP712 EIP712Transaction
        {
            get => new TransactionEIP712
            {
                Salt = Salt,
                SignerAddress = SignerAddress,
                Data = TxData
            };
        }

        public byte[] Hash(EthereumAddress exchangeAddress)
        {
            if (exchangeAddress == null)
                throw new ArgumentNullException(nameof(exchangeAddress));

            byte[] hash = EIP712Service.Hash(EIP712Transaction, GetEIP712Domain(exchangeAddress));

            return hash;
        }

        public byte[] Sign(EthereumAddress exchangeAddress, string privateKey)
        {
            if (exchangeAddress == null)
                throw new ArgumentNullException(nameof(exchangeAddress));
            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));
           EthereumSignature signature = EIP712Service.Sign(EIP712Transaction, GetEIP712Domain(exchangeAddress), privateKey);

            return ByteUtil.Merge(signature.V, signature.R, signature.S, Constants.EIP712SignatureType);
        }

        private EIP712Domain GetEIP712Domain(EthereumAddress exchangeAddress)
            => new EIP712.EIP712Domain
            {
                Name = "0x Protocol",
                Version = "2",
                VerifyingContract = exchangeAddress
            };

    }
}
