using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Assets;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class AssetDataTests
    {

        private static readonly EthereumAddress _testAddress = (EthereumAddress)"0x646934251e9045F41DdF7145Dea5cD63cD45d8Da";

        [TestMethod]
        public void ERC20AssetDataEncoding()
        {
            byte[] assetData = ERC20Asset.Create(_testAddress).AssetData;

            byte[] expectedAssetData = ("0xf47261b0000000000000000000000000646934" +
                "251e9045f41ddf7145dea5cd63cd45d8da").HexToByteArray();

            CollectionAssert.AreEqual(expectedAssetData, assetData);
        }

        [TestMethod]
        public void ERC721AssetDataEncoding()
        {
            byte[] assetData = ERC721Asset.Create(_testAddress, 1).AssetData;

            byte[] expectedAssetData = ("0x02571792000000000000000000000000646934251e9045f41ddf7145dea5cd63cd45d8da00" +
                "00000000000000000000000000000000000000000000000000000000000001").HexToByteArray();

            CollectionAssert.AreEqual(expectedAssetData, assetData);
        }

        [TestMethod]
        public void ERC20AssetCreationFromAssetData()
        {
            byte[] assetData = ("0xf47261b0000000000000000000000000646934" +
                "251e9045f41ddf7145dea5cd63cd45d8da").HexToByteArray();

            ERC20Asset asset = ERC20Asset.Create(assetData);
        }

    }
}
