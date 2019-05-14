using EIP712.Attributes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace ZeroX.Orders
{
    [StructName("Order")]
    internal class OrderInternal
    {
        [Member("address", 1)]
        [Parameter("address", 1)]
        public string MakerAddress { get; set; }

        [Member("address", 2)]
        [Parameter("address", 2)]
        public string TakerAddress { get; set; }

        [Member("address", 3)]
        [Parameter("address", 3)]
        public string FeeRecipientAddress { get; set; }

        [Member("address", 4)]
        [Parameter("address", 4)]
        public string SenderAddress { get; set; }

        [Member("uint256", 5)]
        [Parameter("uint256", 5)]
        public BigInteger MakerAssetAmount { get; set; }

        [Member("uint256", 6)]
        [Parameter("uint256", 6)]
        public BigInteger TakerAssetAmount { get; set; }

        [Member("uint256", 7)]
        [Parameter("uint256", 7)]
        public BigInteger MakerFee { get; set; }

        [Member("uint256", 8)]
        [Parameter("uint256", 8)]
        public BigInteger TakerFee { get; set; }

        [Member("uint256", 9)]
        [Parameter("uint256", 9)]
        public BigInteger ExpirationTimeSeconds { get; set; }

        [Member("uint256", 10)]
        [Parameter("uint256", 10)]
        public BigInteger Salt { get; set; }

        [Member("bytes", 11)]
        [Parameter("bytes", 11)]
        public byte[] MakerAssetData { get; set; }

        [Member("bytes", 12)]
        [Parameter("bytes", 12)]
        public byte[] TakerAssetData { get; set; }
    }
}
