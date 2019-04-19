using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Assets;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class AssetDataTests
    {

        private static readonly EthereumAddress _testAddress = (EthereumAddress)Constants.TestEthereumAddress;
        private static readonly byte[] _testERC20AssetData = ("0xf47261b0000000000000000000000000646934" +
                "251e9045f41ddf7145dea5cd63cd45d8da").HexToByteArray();

        private static readonly byte[] _testERC721AssetData = ("0x0257179200000000000000000000000064693425" +
            "1e9045f41ddf7145dea5cd63cd45d8da00" +"00000000000000000000000000000000000000000000000000000000000001").HexToByteArray();

        [TestMethod]
        public void ERC20AssetDataEncoding()
        {
            byte[] assetData = ERC20Asset.Create(_testAddress).AssetData;
            byte[] expectedAssetData = _testERC20AssetData;
            CollectionAssert.AreEqual(_testERC20AssetData, assetData);
        }

        [TestMethod]
        public void ERC721AssetDataEncoding()
        {
            byte[] assetData = ERC721Asset.Create(_testAddress, 1).AssetData;
            CollectionAssert.AreEqual(_testERC721AssetData, assetData);
        }

        [TestMethod]
        public void ERC20AssetCreationFromAssetData()
        {
            ERC20Asset asset = ERC20Asset.Create(_testERC20AssetData);
            Assert.AreEqual(_testAddress, asset.TokenAddress);
        }

        [TestMethod]
        public void ERC721AssetCreationFromAssetData()
        {
            ERC721Asset asset = ERC721Asset.Create(_testERC721AssetData);
            Assert.AreEqual(1, asset.TokenId);
            Assert.AreEqual((EthereumAddress)(Constants.TestEthereumAddress), asset.TokenAddress);
        }

    }
}
