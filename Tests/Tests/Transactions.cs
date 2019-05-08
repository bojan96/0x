using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using ZeroX.Transactions;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class Transactions
    {
        [TestMethod]
        public void HashTransaction()
        {
            Transaction tx = new Transaction(EthereumAddress.ZeroAddress, "0x00");
            byte[] hash = tx.Hash(EthereumAddress.ZeroAddress);
            byte[] expectedHash = "0x260d10cec3971fd8a2037ea41e9b4f2658fb1bbf15984ae8428c2f8f6e3408ee".HexToByteArray();

            CollectionAssert.AreEqual(expectedHash, hash);
        }
    }
}
