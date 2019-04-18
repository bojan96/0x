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
        public Asset MakerAssetData { get; set; }
        public Asset TakerAssetData { get; set; }

        public byte[] Hash(EthereumAddress exchangeAddress)
        {
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
                MakerFee = TakerFee,
                ExpirationTimeSeconds = ExpirationTimeSeconds,
                Salt = Salt,
                MakerAssetData = MakerAssetData.AssetData,
                TakerAssetData = TakerAssetData.AssetData
            }; 
        }
    }
}
