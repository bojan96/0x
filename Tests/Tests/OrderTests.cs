using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Assets;
using ZeroX.Orders;
using ZeroX.Utilities;
using System;

namespace Tests.Tests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Hash()
        {
            Order order = new Order
            {
                MakerAddress = EthereumAddress.ZeroAddress,
                TakerAddress = EthereumAddress.ZeroAddress,
                FeeRecipientAddress = EthereumAddress.ZeroAddress,
                SenderAddress = EthereumAddress.ZeroAddress,
                MakerAssetAmount = 1,
                TakerAssetAmount = 1,
                MakerFee = 1,
                TakerFee = 1,
                Salt = 1,
                ExpirationTime = new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc),
                MakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress),
                TakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress)
            };

            byte[] hash = order.Hash(EthereumAddress.ZeroAddress);

            byte[] expectedHash = "0xd81bf9ae7657b4fc194c63e3ed26153f05032ed0173df4069ddf619f0b26c888"
                .HexToByteArray();

            CollectionAssert.AreEqual(expectedHash, hash);
        }

        [TestMethod]
        public void VerifySignature()
        {

            Order order = new Order
            {
                MakerAddress = EthereumAddress.ZeroAddress,
                TakerAddress = EthereumAddress.ZeroAddress,
                FeeRecipientAddress = EthereumAddress.ZeroAddress,
                SenderAddress = EthereumAddress.ZeroAddress,
                MakerAssetAmount = 1,
                TakerAssetAmount = 1,
                MakerFee = 1,
                TakerFee = 1,
                Salt = 1,
                ExpirationTime = new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc),
                MakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress),
                TakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress)
            };

            byte[] signature = order.Sign(EthereumAddress.ZeroAddress, Constants.TestPrivateKey);
            bool valid = order.VerifySignature(EthereumAddress.ZeroAddress, 
                (EthereumAddress)Constants.TestEthereumAddress, signature);

            Assert.IsTrue(valid);

        }


    }
}
