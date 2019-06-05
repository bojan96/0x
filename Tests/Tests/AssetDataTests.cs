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
        private static readonly byte[] _testERC20AssetData = ("0xf47261b0000000000000000000000" +
            "0005409ed021d9299bf6814279a6a1411a7e866a631").HexToByteArray();
        private static readonly byte[] _testERC721AssetData = ("0x0257179200000000000" +
            "00000000000005409ed021d9299bf6814279a6a1411a7e866a6310000000000000000000" +
            "000000000000000000000000000000000000000000001").HexToByteArray();

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
            Assert.AreEqual(_testAddress, asset.TokenAddress);
        }

        [TestMethod]
        public void AssetUtilCreateERC20AssetData()
        {
            ERC20Asset asset = AssetUtil.Create<ERC20Asset>(_testERC20AssetData);
            Assert.AreEqual(_testAddress, asset.TokenAddress);
        }

        [TestMethod]
        public void AssetUtilCreateERC721AssetData()
        {
            ERC721Asset asset = AssetUtil.Create<ERC721Asset>(_testERC721AssetData);
            Assert.AreEqual(_testAddress, asset.TokenAddress);
            Assert.AreEqual(1, asset.TokenId);
        }
    }
}
