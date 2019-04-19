using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class EthereumAddressTests
    {
        [TestMethod]
        public void Construction()
        {
            EthereumAddress address = new EthereumAddress(Constants.TestEthereumAddress);
            Assert.AreEqual(Constants.TestEthereumAddress, address.ToString());
            Assert.IsTrue(address.IsChecksumAddress, "Address should be checksummed");
        }


        [TestMethod]
        public void Equality()
        {
            EthereumAddress a1 = new EthereumAddress(Constants.TestEthereumAddress);
            EthereumAddress a2 = new EthereumAddress(Constants.TestEthereumAddress);

            Assert.AreEqual(a1, a2);
            Assert.AreEqual(a2, a1);
            Assert.AreEqual(a1, a1);
            Assert.AreEqual(a2, a2);
#pragma warning disable CS1718
            Assert.IsTrue(a1 == a1);
            Assert.IsTrue(a2 == a2);
#pragma warning restore CS1718
            Assert.IsTrue(a1 == a2);
            Assert.IsTrue(a2 == a1);
        }

        [TestMethod]
        public void Inequality()
        {
            EthereumAddress a = new EthereumAddress(Constants.TestEthereumAddress);

            Assert.AreNotEqual(a, EthereumAddress.ZeroAddress);
            Assert.AreNotEqual(EthereumAddress.ZeroAddress, a);
            Assert.IsTrue(a != EthereumAddress.ZeroAddress);
            Assert.IsTrue(EthereumAddress.ZeroAddress != a);
        }

        [TestMethod]
        public void EqualityChecksum()
        {
            // Checksum should not affect equality of addresses
            EthereumAddress a = new EthereumAddress(Constants.TestEthereumAddress);
            EthereumAddress nonChecksumAddr = new EthereumAddress(Constants.TestEthereumAddress.ToLowerInvariant());

            Assert.AreEqual(a, nonChecksumAddr);
            Assert.IsTrue(a == nonChecksumAddr);
        }

        [TestMethod]
        public void Conversions()
        {
            EthereumAddress a = new EthereumAddress(Constants.TestEthereumAddress);
            Assert.AreEqual(Constants.TestEthereumAddress, a.ToString());

            // Tests explicit cast operator
            Assert.AreEqual(a, (EthereumAddress)Constants.TestEthereumAddress);
            // Tests implicit cast operator
            string aStr = a;
            Assert.AreEqual(Constants.TestEthereumAddress, aStr);

            byte[] expectedBytes = a.Bytes;
            CollectionAssert.AreEqual(Constants.TestEthereumAddress.HexToByteArray(), a.Bytes);
            Assert.AreEqual(a, EthereumAddress.FromByteArray(a.Bytes));
        }

    }
}
