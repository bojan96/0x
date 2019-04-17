using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class AssetDataUtilsTest
    {
        [TestMethod]
        public void TestERC20AssetDataEncoding()
        {
            byte[] assetData = AssetDataUtils.GetERC20AssetData(EthereumAddress.ZeroAddress);

            byte[] expectedAssetData = ("0xf47261b00000000000000000000000000000000000000000000" +
                "000000000000000000000").HexToByteArray();

            CollectionAssert.AreEqual(expectedAssetData, assetData);
        }

    }
}
