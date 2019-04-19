using EIP712;
using System.Numerics;
using ZeroX.Assets;
using ZeroX.Utilities;

namespace ZeroX.Orders
{
    public class Order
    {
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

        public byte[] Hash(EthereumAddress exchangeAddress)
        {
            EIP712Domain domain = new EIP712Domain()
            {
                Name = "0x Protocol",
                Version = "2",
                VerifyingContract = exchangeAddress.ToString()
            };

            var ord = EIP712Order;
            var res = EIP712.EIP712.Encode(ord, domain);
            return EIP712.EIP712.Hash(ord, domain);
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
