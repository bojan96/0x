using EIP712;
using System;
using System.Numerics;
using ZeroX.Assets;
using ZeroX.Utilities;

namespace ZeroX.Orders
{
    public class Order
    {
        // TODO: Add docs, property validation

        public EthereumAddress MakerAddress { get; set; }
        public EthereumAddress TakerAddress { get; set; }
        public EthereumAddress FeeRecipientAddress { get; set; }
        public EthereumAddress SenderAddress { get; set; }
        public BigInteger MakerAssetAmount { get; set; }
        public BigInteger TakerAssetAmount { get; set; }
        public BigInteger MakerFee { get; set; }
        public BigInteger TakerFee { get; set; }
        public BigInteger ExpirationTimeSeconds { get; set; }
        public BigInteger Salt { get; set; }
        public Asset MakerAsset { get; set; }
        public Asset TakerAsset { get; set; }

        /// <summary>
        /// Hashes given order
        /// </summary>
        /// <param name="exchangeAddress">Address of Exchange contract</param>
        /// <returns>Order signature</returns>
        /// <exception cref="ArgumentNullException">exchangeAddress is null</exception>
        public byte[] Hash(EthereumAddress exchangeAddress)
        {
            if (exchangeAddress == null)
                throw new ArgumentNullException(nameof(exchangeAddress));

            EIP712Domain domain = new EIP712Domain()
            {
                Name = "0x Protocol",
                Version = "2",
                VerifyingContract = exchangeAddress.ToString()
            };

            return EIP712.EIP712.Hash(EIP712Order, domain);
        }

        private OrderInternal EIP712Order
        {
            get => new OrderInternal
            {
                MakerAddress = MakerAddress.ToString(),
                TakerAddress = TakerAddress.ToString(),
                FeeRecipientAddress = FeeRecipientAddress.ToString(),
                SenderAddress = SenderAddress.ToString(),
                MakerAssetAmount = MakerAssetAmount,
                TakerAssetAmount = TakerAssetAmount,
                MakerFee = MakerFee,
                TakerFee = TakerFee,
                ExpirationTimeSeconds = ExpirationTimeSeconds,
                Salt = Salt,
                MakerAssetData = MakerAsset.AssetData,
                TakerAssetData = TakerAsset.AssetData
            }; 
        }
    }
}
